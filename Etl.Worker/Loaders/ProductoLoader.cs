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
            foreach (var producto in data)
            {
                var existe = await _db.Dim_Producto
                    .AnyAsync(p => p.Codigo_Producto == producto.Codigo_Producto, ct);

                if (!existe)
                {
                    await _db.Dim_Producto.AddAsync(producto, ct);
                }
            }
            await _db.SaveChangesAsync(ct);
        }
    }
}
