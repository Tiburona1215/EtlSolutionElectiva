using Etl.Core.Innterface;
using Etl.Core.Models;
using Etl.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Worker.Loaders
{
    public class EfLoader : ILoader<FactVent>
    {
        private readonly EtlDbContext _db;

        public EfLoader(EtlDbContext db) => _db = db;

        public async Task LoadAsync(IEnumerable<FactVent> data, CancellationToken ct = default)
        {
            await _db.Fact_Vent.AddRangeAsync(data, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}
