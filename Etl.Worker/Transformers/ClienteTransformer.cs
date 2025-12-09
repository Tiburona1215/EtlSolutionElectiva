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
    public class ClienteTransformer : ITransformer<CustomerDto, DimCliente>
    {
        public Task<IEnumerable<DimCliente>> TransformAsync(IEnumerable<CustomerDto> input, CancellationToken ct = default)
        {
            var transformed = input.Select(dto => new DimCliente
            {
                ID_Cliente = dto.CustomerID,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Pais = dto.Country,
                Ciudad = dto.City,
                Correo = dto.Email,
                Telefono = dto.Phone,
                Fecha_Registro = DateTime.Now
            });
            return Task.FromResult(transformed);
        }
    }
}
