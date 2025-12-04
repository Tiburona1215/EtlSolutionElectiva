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
    public class TiendaTransformer : ITransformer<OrderDto, DimTienda>
    {
        public Task<IEnumerable<DimTienda>> TransformAsync(IEnumerable<OrderDto> input, CancellationToken ct = default)
        {
            var transformed = input
                .GroupBy(o => new { o.StoreID, o.StoreName, o.ShipCity, o.ShipCountry, o.ShipRegion })
                .Select(g => new DimTienda
                {
                    Nombre_Tienda = g.Key.StoreName,
                    Pais = g.Key.ShipCountry,
                    Region = g.Key.ShipRegion,
                    Ciudad = g.Key.ShipCity,
                    Direccion = "",
                    Tipo_Tienda = "Física"
                });
            return Task.FromResult(transformed);
        }
    }
}
