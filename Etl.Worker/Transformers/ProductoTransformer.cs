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
                Marca = dto.Brand,
                Categoria = dto.Category,
                Precio_Act = dto.UnitPrice,
                Costo = dto.Cost,
                Unidad_Medida = dto.Unit,
                Estado = dto.Status,
                Fuente = "CSV"
            });
            return Task.FromResult(transformed);
        }
    }
}
