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
            var existingKeys = await _db.Dim_Tiempo
                 .Select(t => t.FechaKey)
                 .ToListAsync(ct);

            var existingKeySet = new HashSet<int>(existingKeys);

            var newTiempos = data
                .Where(tiempo => !existingKeySet.Contains(tiempo.FechaKey));

            await _db.Dim_Tiempo.AddRangeAsync(newTiempos, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}
