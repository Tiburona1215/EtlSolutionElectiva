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
            var clienteKeyLookup = await _db.Dim_Cliente
                .ToDictionaryAsync(c => c.ID_Cliente, c => c.ClienteKey, ct);

            var productoLookup = await _db.Dim_Producto
                .ToDictionaryAsync(
                    p => p.Codigo_Producto,
                    p => new { p.ProductKey, p.Costo },
                    ct);

            var fechaKeyLookup = await _db.Dim_Tiempo
                .ToDictionaryAsync(t => t.Fecha_Complet.Date, t => t.FechaKey, ct);

            var orderLookup = orders.ToDictionary(o => o.OrderID);


            var facts = new List<FactVent>();
            foreach (var detail in orderDetails)
            {
                if (!orderLookup.TryGetValue(detail.OrderID, out var order) || order == null)
                    continue;

                if (!clienteKeyLookup.TryGetValue(order.CustomerID, out var clienteKey))
                    continue;

                if (!productoLookup.TryGetValue(detail.ProductID, out var productoData))
                    continue;

                var productoKey = productoData.ProductKey;
                var costoUnitario = productoData.Costo;

                if (!fechaKeyLookup.TryGetValue(order.OrderDate.Date, out var fechaKey))
                    continue;

                var costo = detail.Quantity * costoUnitario;

                facts.Add(new FactVent
                {
                    FechaKey = fechaKey,
                    ProductKey = productoKey,
                    ClienteKey = clienteKey,
                    Cantidad_Vendida = detail.Quantity,
                    Precio_Unitario = detail.TotalPrice,
                    Costo = costo,
                    Moneda = "USD",
                    Fuente = "CSV"
                });
            }

            return facts;
        }

    }
}
