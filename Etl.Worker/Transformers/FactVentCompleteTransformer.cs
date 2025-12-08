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
        }
    }
}
