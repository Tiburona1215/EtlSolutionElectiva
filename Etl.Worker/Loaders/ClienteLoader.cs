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
            foreach (var cliente in data)
            {
                var existe = await _db.Dim_Cliente
                    .AnyAsync(c => c.ID_Cliente == cliente.ID_Cliente, ct);

                if (!existe)
                {
                    await _db.Dim_Cliente.AddAsync(cliente, ct);
                }
            }
            await _db.SaveChangesAsync(ct);
        }
    }
}
