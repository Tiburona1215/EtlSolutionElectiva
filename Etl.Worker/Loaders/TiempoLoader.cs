using Etl.Core.Innterface;
using Etl.Core.Models;
using Etl.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Worker.Loaders
{
    public class TiempoLoader : ILoader<DimTiempo>
    {
        private readonly EtlDbContext _db;

        public TiempoLoader(EtlDbContext db) => _db = db;

        public async Task LoadAsync(IEnumerable<DimTiempo> data, CancellationToken ct = default)
        {
            foreach (var tiempo in data)
            {
                //var existe = await _db.Dim_Tiempo
                //    .AnyAsync(t => t.Fecha_Complet.Date == tiempo.Fecha_Complet.Date, ct);
                var existe = await _db.Dim_Tiempo
                    .AnyAsync(t => t.FechaKey == tiempo.FechaKey, ct);

                if (!existe)
                {
                    await _db.Dim_Tiempo.AddAsync(tiempo, ct);
                }
            }
            await _db.SaveChangesAsync(ct);
        }
    }
}
