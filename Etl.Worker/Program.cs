using Etl.Core.Innterface;
using Etl.Core.Models;
using Etl.Core.Models.Dtos;
using Etl.Data;
using Etl.Infrastructure.Estractors;
using Etl.Worker;
using Etl.Worker.Loaders;
using Etl.Worker.Transformers;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var connectionString = ctx.Configuration.GetConnectionString("MainDb")
            ?? "Server=LENOVO12JANABJ;Database=DWSisVentas;Integrated Security=true;TrustServerCertificate=true";

        services.AddDbContext<EtlDbContext>(opt =>
            opt.UseSqlServer(connectionString));

        string baseDir = Path.Combine(AppContext.BaseDirectory, "Data");
        string customersPath = Path.Combine(baseDir, "customers.csv");
        string productsPath = Path.Combine(baseDir, "products.csv");
        string ordersPath = Path.Combine(baseDir, "orders.csv");
        string orderDetailsPath = Path.Combine(baseDir, "order_details.csv");

        services.AddTransient<IExtractor<CustomerDto>>(sp =>
            new CsvExtractor<CustomerDto>(customersPath));
        services.AddTransient<IExtractor<ProductDto>>(sp =>
            new CsvExtractor<ProductDto>(productsPath));
        services.AddTransient<IExtractor<OrderDto>>(sp =>
            new CsvExtractor<OrderDto>(ordersPath));
        services.AddTransient<IExtractor<OrderDetailDto>>(sp =>
            new CsvExtractor<OrderDetailDto>(orderDetailsPath));

        services.AddTransient<ITransformer<CustomerDto, DimCliente>, ClienteTransformer>();
        services.AddTransient<ITransformer<ProductDto, DimProducto>, ProductoTransformer>();
        services.AddTransient<ITransformer<OrderDto, DimTiempo>, TiempoTransformer>();
        services.AddTransient<FactVentCompleteTransformer>();

        services.AddTransient<ILoader<DimCliente>, ClienteLoader>();
        services.AddTransient<ILoader<DimProducto>, ProductoLoader>();
        services.AddTransient<ILoader<DimTiempo>, TiempoLoader>();
        services.AddTransient<ILoader<FactVent>, EfLoader>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();