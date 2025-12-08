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
    public class ProductoTransformer : ITransformer<ProductDto, DimProducto>
    {
        public Task<IEnumerable<DimProducto>> TransformAsync(IEnumerable<ProductDto> input, CancellationToken ct = default)
        {
            var transformed = input.Select(dto => new DimProducto
            {
                Codigo_Producto = dto.ProductID,
                Nombre_Producto = dto.ProductName,
                Categoria = dto.Category,
                Precio_Act = dto.Price,
                Costo = 0,
                Unidad_Medida = "",
                Estado = "Activo",
                Fuente = "CSV"
            });
            return Task.FromResult(transformed);
        }
    }
}
