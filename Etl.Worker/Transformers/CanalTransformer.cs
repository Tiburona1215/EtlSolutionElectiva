using Etl.Core.Innterface;
using Etl.Core.Models;
using Etl.Core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Worker.Transformers
{
    public class CanalTransformer : ITransformer<OrderDto, DimCanal>
    {
        public Task<IEnumerable<DimCanal>> TransformAsync(IEnumerable<OrderDto> input, CancellationToken ct = default)
        {
            var transformed = input
                .GroupBy(o => o.Channel)
                .Select(g => new DimCanal
                {
                    Nombre_Canal = g.Key,
                    Descripcion = $"Canal de venta {g.Key}"
                });
            return Task.FromResult(transformed);
        }
    }
}
