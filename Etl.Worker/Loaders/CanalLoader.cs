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
    public class CanalLoader : ILoader<DimCanal>
    {
        private readonly EtlDbContext _db;

        public CanalLoader(EtlDbContext db) => _db = db;

        public async Task LoadAsync(IEnumerable<DimCanal> data, CancellationToken ct = default)
        {
            foreach (var canal in data)
            {
                var existe = await _db.Dim_Canal
                    .AnyAsync(c => c.Nombre_Canal == canal.Nombre_Canal, ct);

                if (!existe)
                {
                    await _db.Dim_Canal.AddAsync(canal, ct);
                }
            }
            await _db.SaveChangesAsync(ct);
        }
    }
}
