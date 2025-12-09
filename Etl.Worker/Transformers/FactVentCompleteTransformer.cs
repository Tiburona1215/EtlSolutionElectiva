using Etl.Core.Models;
using Etl.Core.Models.Dtos;
using Etl.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Worker.Transformers
{
    public class FactVentCompleteTransformer
    {
        private readonly EtlDbContext _db;

        public FactVentCompleteTransformer(EtlDbContext db) => _db = db;

        public async Task<IEnumerable<FactVent>> TransformAsync(
            IEnumerable<OrderDto> orders,
            IEnumerable<OrderDetailDto> orderDetails,
            CancellationToken ct = default)
        {
            // ====================================================================
            // PASO DE OPTIMIZACIÓN: Pre-cargar todas las claves de dimensión en memoria
            // ====================================================================

            // 1. Claves de Clientes: Map ID_Cliente (Source) a ClienteKey (DW)
            var clienteKeyLookup = await _db.Dim_Cliente
                .ToDictionaryAsync(c => c.ID_Cliente, c => c.ClienteKey, ct);

            // 2. Claves de Productos y Costo: Map Codigo_Producto (Source) a (ProductKey, Costo)
            var productoLookup = await _db.Dim_Producto
                .ToDictionaryAsync(
                    p => p.Codigo_Producto,
                    p => new { p.ProductKey, p.Costo },
                    ct);

            // 3. Claves de Tiempo: Map Fecha_Complet.Date (Source) a FechaKey (DW)
            var fechaKeyLookup = await _db.Dim_Tiempo
                .ToDictionaryAsync(t => t.Fecha_Complet.Date, t => t.FechaKey, ct);

            // 4. Transformar la lista de órdenes a un diccionario para un acceso O(1)
            var orderLookup = orders.ToDictionary(o => o.OrderID);


            var facts = new List<FactVent>();

            // ====================================================================
            // Bucle de Transformación: Usa búsquedas en memoria
            // ====================================================================
            foreach (var detail in orderDetails)
            {
                // Reemplaza FirstOrDefault por una búsqueda rápida en el diccionario
                if (!orderLookup.TryGetValue(detail.OrderID, out var order) || order == null)
                    continue;

                // === Búsquedas en memoria (O(1)) ===

                // 1. Cliente Key
                if (!clienteKeyLookup.TryGetValue(order.CustomerID, out var clienteKey))
                    continue;

                // 2. Producto Key y Costo
                if (!productoLookup.TryGetValue(detail.ProductID, out var productoData))
                    continue;

                var productoKey = productoData.ProductKey;
                var costoUnitario = productoData.Costo;

                // 3. Fecha Key
                if (!fechaKeyLookup.TryGetValue(order.OrderDate.Date, out var fechaKey))
                    continue;

                // === CÁLCULO DE HECHOS ===

                // Eliminado el await _db.Dim_Producto.FindAsync(productoKey); redundante
                var costo = detail.Quantity * costoUnitario;

                facts.Add(new FactVent
                {
                    FechaKey = fechaKey,
                    ProductKey = productoKey,
                    ClienteKey = clienteKey,
                    Cantidad_Vendida = detail.Quantity,
                    Precio_Unitario = detail.TotalPrice,
                    // Total_Venta, Ganancia (comentados en el original)
                    Costo = costo,
                    Moneda = "USD",
                    Fuente = "CSV"
                });
            }

            return facts;
        }

        /*private readonly EtlDbContext _db;

        public FactVentCompleteTransformer(EtlDbContext db) => _db = db;

        public async Task<IEnumerable<FactVent>> TransformAsync(
            IEnumerable<OrderDto> orders,
            IEnumerable<OrderDetailDto> orderDetails,
            CancellationToken ct = default)
        {
            var facts = new List<FactVent>();

            foreach (var detail in orderDetails)
            {
                var order = orders.FirstOrDefault(o => o.OrderID == detail.OrderID);
                if (order == null) continue;

                var clienteKey = await _db.Dim_Cliente
                    .Where(c => c.ID_Cliente == order.CustomerID)
                    .Select(c => c.ClienteKey)
                    .FirstOrDefaultAsync(ct);

                var productoKey = await _db.Dim_Producto
                    .Where(p => Convert.ToInt32(p.Codigo_Producto) == detail.ProductID)
                    .Select(p => p.ProductKey)
                    .FirstOrDefaultAsync(ct);

                //var canalKey = await _db.Dim_Canal
                //    .Where(c => c.Nombre_Canal == order.Channel)
                //    .Select(c => c.CanalKey)
                //    .FirstOrDefaultAsync(ct);

                var fechaKey = await _db.Dim_Tiempo
                    .Where(t => t.Fecha_Complet.Date == order.OrderDate.Date)
                    .Select(t => t.FechaKey)
                    .FirstOrDefaultAsync(ct);

                //if (clienteKey == 0 || productoKey == 0 || tiendaKey == 0 ||
                //    canalKey == 0 || fechaKey == 0)
                //    continue;

                var producto = await _db.Dim_Producto.FindAsync(productoKey);
                //var totalVenta = detail.Quantity * detail.TotalPrice * (1 - detail.Discount);
                var costo = detail.Quantity * (producto?.Costo ?? 0);

                facts.Add(new FactVent
                {
                    FechaKey = fechaKey,
                    ProductKey = productoKey,
                    ClienteKey = clienteKey,
                    Cantidad_Vendida = detail.Quantity,
                    Precio_Unitario = detail.TotalPrice,
                  //  Total_Venta = totalVenta,
                    Costo = costo,
                  //  Ganancia = totalVenta - costo,
                    Moneda = "USD",
                    Fuente = "CSV"
                });
            }

            return facts;
        }*/
    }
}
