using Etl.Core.Innterface;
using Etl.Core.Models;
using Etl.Core.Models.Dtos;
using Etl.Data;
using Etl.Worker.Transformers;
using Microsoft.EntityFrameworkCore;

namespace Etl.Worker;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IServiceProvider serviceProvider,
        ILogger<Worker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("=== ETL INICIADO ===");

            // Crear un scope para resolver servicios que dependen de DbContext
            using var scope = _serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            // Resolver todos los servicios del scope
            var customerExtractor = services.GetRequiredService<IExtractor<CustomerDto>>();
            var productExtractor = services.GetRequiredService<IExtractor<ProductDto>>();
            var orderExtractor = services.GetRequiredService<IExtractor<OrderDto>>();
            var orderDetailExtractor = services.GetRequiredService<IExtractor<OrderDetailDto>>();

            var clienteTransformer = services.GetRequiredService<ITransformer<CustomerDto, DimCliente>>();
            var productoTransformer = services.GetRequiredService<ITransformer<ProductDto, DimProducto>>();
            var tiempoTransformer = services.GetRequiredService<ITransformer<OrderDto, DimTiempo>>();

            var clienteLoader = services.GetRequiredService<ILoader<DimCliente>>();
            var productoLoader = services.GetRequiredService<ILoader<DimProducto>>();
            var tiempoLoader = services.GetRequiredService<ILoader<DimTiempo>>();
            var factLoader = services.GetRequiredService<ILoader<FactVent>>();

            var factTransformer = services.GetRequiredService<FactVentCompleteTransformer>();

            // 1. CARGAR DIMENSIÓN CLIENTES
            _logger.LogInformation("1. Cargando dimensión Clientes...");
            var customers = await customerExtractor.ExtractAsync(stoppingToken);
            var clientes = await clienteTransformer.TransformAsync(customers, stoppingToken);
            await clienteLoader.LoadAsync(clientes, stoppingToken);
            _logger.LogInformation($"   ? {clientes.Count()} clientes cargados");

            // 2. CARGAR DIMENSIÓN PRODUCTOS
            _logger.LogInformation("2. Cargando dimensión Productos...");
            var products = await productExtractor.ExtractAsync(stoppingToken);
            var productos = await productoTransformer.TransformAsync(products, stoppingToken);
            await productoLoader.LoadAsync(productos, stoppingToken);
            _logger.LogInformation($"   ? {productos.Count()} productos cargados");

            // 3. CARGAR DIMENSIONES DE ÓRDENES
            _logger.LogInformation("3. Cargando dimensiones de Órdenes...");
            var orders = await orderExtractor.ExtractAsync(stoppingToken);

            var tiempos = await tiempoTransformer.TransformAsync(orders, stoppingToken);
            await tiempoLoader.LoadAsync(tiempos, stoppingToken);
            _logger.LogInformation($"   ? {tiempos.Count()} fechas cargadas");

            // 4. CARGAR HECHOS (FACT TABLE)
            _logger.LogInformation("4. Cargando hechos de ventas...");
            var orderDetails = await orderDetailExtractor.ExtractAsync(stoppingToken);
            var facts = await factTransformer.TransformAsync(orders, orderDetails, stoppingToken);
            await factLoader.LoadAsync(facts, stoppingToken);
            _logger.LogInformation($"   ? {facts.Count()} registros de ventas cargados");

            _logger.LogInformation("=== ETL COMPLETADO EXITOSAMENTE ===");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el proceso ETL");
            throw;
        }
    }
    }