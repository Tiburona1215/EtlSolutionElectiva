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
    public class ClienteLoader : ILoader<DimCliente>
    {
        private readonly EtlDbContext _db;

        public ClienteLoader(EtlDbContext db) => _db = db;

        public async Task LoadAsync(IEnumerable<DimCliente> data, CancellationToken ct = default)
        {
            var existingKeys = await _db.Dim_Cliente.Select(c => c.ID_Cliente) .ToListAsync(ct);
            var existingKeySet = new HashSet<int>(existingKeys);
            var newClients = data .Where(cliente => !existingKeySet.Contains(cliente.ID_Cliente));

            await _db.Dim_Cliente.AddRangeAsync(newClients, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}
