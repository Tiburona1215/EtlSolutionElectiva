using Etl.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Data
{
    public class EtlDbContext : DbContext
    {
        public EtlDbContext(DbContextOptions<EtlDbContext> options) : base(options) { }

        public DbSet<DimProducto> Dim_Producto => Set<DimProducto>();
        public DbSet<DimCliente> Dim_Cliente => Set<DimCliente>();
        public DbSet<DimTiempo> Dim_Tiempo => Set<DimTiempo>();
        public DbSet<FactVent> Fact_Vent => Set<FactVent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FactVent>()
                .HasOne(f => f.Producto)
                .WithMany(p => p.Ventas)
                .HasForeignKey(f => f.ProductKey);

            modelBuilder.Entity<FactVent>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(f => f.ClienteKey);

            modelBuilder.Entity<FactVent>()
                .HasOne(f => f.Tiempo)
                .WithMany(t => t.Ventas)
                .HasForeignKey(f => f.FechaKey);

            modelBuilder.Entity<DimTiempo>()
                .Property(t => t.FechaKey)
                .ValueGeneratedNever();

        }
    }
}
