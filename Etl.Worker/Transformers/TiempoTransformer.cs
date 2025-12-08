using Etl.Core.Innterface;
using Etl.Core.Models;
using Etl.Core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Worker.Transformers
{
    public class TiempoTransformer : ITransformer<OrderDto, DimTiempo>
    {
        public Task<IEnumerable<DimTiempo>> TransformAsync(IEnumerable<OrderDto> input, CancellationToken ct = default)
        {
            var transformed = input
                .Select(o => o.OrderDate.Date)
                .Distinct()
                .Select(fecha => new DimTiempo
                {
                    FechaKey = int.Parse(fecha.ToString("yyyyMMdd")),
                    Fecha_Complet = fecha,
                    Año = fecha.Year,
                    Mes = fecha.Month,
                    Trimestre = (fecha.Month - 1) / 3 + 1,
                    Día = fecha.Day,
                    Nombre_Mes = fecha.ToString("MMMM", new CultureInfo("es-ES"))
                });
            return Task.FromResult(transformed);
        }
    }
}
