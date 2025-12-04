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
    public class TiendaLoader : ILoader<DimTienda>
    {
        private readonly EtlDbContext _db;

        public TiendaLoader(EtlDbContext db) => _db = db;

        public async Task LoadAsync(IEnumerable<DimTienda> data, CancellationToken ct = default)
        {
            foreach (var tienda in data)
            {
                var existe = await _db.Dim_Tienda
                    .AnyAsync(t => t.Nombre_Tienda == tienda.Nombre_Tienda &&
                                   t.Ciudad == tienda.Ciudad, ct);

                if (!existe)
                {
                    await _db.Dim_Tienda.AddAsync(tienda, ct);
                }
            }
            await _db.SaveChangesAsync(ct);
        }
    }
}