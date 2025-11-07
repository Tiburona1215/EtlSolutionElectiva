using Etl.Core.Innterface;
using Etl.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Worker.Transformers
{
    public class FactTransformer : ITransformer<RawVentaDto, FactVent>
    {
        public Task<IEnumerable<FactVent>> TransformAsync(IEnumerable<RawVentaDto> input, CancellationToken ct = default)
        {
            var transformed = input.Select(dto => new FactVent
            {
                FechaKey = dto.FechaKey,
                ProductKey = dto.ProductKey,
                ClienteKey = dto.ClienteKey,
                TiendaKey = dto.TiendaKey,
                CanalKey = dto.CanalKey,
                Cantidad_Vendida = dto.Cantidad_Vendida,
                Precio_Unitario = dto.Precio_Unitario,
                Descuento = dto.Descuento,
                Total_Venta = dto.Total_Venta,
                Costo = dto.Costo,
                Ganancia = dto.Ganancia,
                Moneda = dto.Moneda,
                Fuente = "CSV"
            });
            return Task.FromResult(transformed);
        }
    }
}
