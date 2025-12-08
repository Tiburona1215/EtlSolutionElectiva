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
            var transformed = new List<DimCanal>
        {
            new DimCanal
            {
                Nombre_Canal = "Online",
                Descripción = "Venta realizada a través del sitio web"
            },
            new DimCanal
            {
                Nombre_Canal = "Físico",
                Descripción = "Venta realizada en tienda física"
            }
        };
            return Task.FromResult(transformed.AsEnumerable());
        }
    }
}
