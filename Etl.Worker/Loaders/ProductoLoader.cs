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
    public class ProductoLoader : ILoader<DimProducto>
    {
        private readonly EtlDbContext _db;

        public ProductoLoader(EtlDbContext db) => _db = db;

        public async Task LoadAsync(IEnumerable<DimProducto> data, CancellationToken ct = default)
        {
            var existingKeys = await _db.Dim_Producto
                 .Select(p => p.Codigo_Producto)
                 .ToListAsync(ct);

            var existingKeySet = new HashSet<int>(existingKeys);
            var newProducts = data
                .Where(producto => !existingKeySet.Contains(producto.Codigo_Producto));
            
            await _db.Dim_Producto.AddRangeAsync(newProducts, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}
