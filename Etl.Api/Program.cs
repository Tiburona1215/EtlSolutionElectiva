using Etl.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EtlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDb")
        ?? "Server=localhost;Database=ETL_DB;Trusted_Connection=True;TrustServerCertificate=True"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region ENDPOINTS

app.MapGet("/api/ventas/rango", async (DateTime desde, DateTime hasta, EtlDbContext db) =>
{
    var ventas = await db.Fact_Vent
        .Where(v => v.Tiempo!.Fecha_Complet >= desde && v.Tiempo!.Fecha_Complet <= hasta)
        .Select(v => new
        {
            v.ID_Venta,
            Fecha = v.Tiempo!.Fecha_Complet,
            Producto = v.Producto!.Nombre_Producto,
            Cliente = v.Cliente!.Nombre_Cliente,
            v.Total_Venta,
            v.Moneda
        })
        .ToListAsync();

    return Results.Ok(ventas);
});

app.MapGet("/api/ventas/top-productos", async (int limite, EtlDbContext db) =>
{
    var ranking = await db.Fact_Vent
        .GroupBy(v => v.Producto!.Nombre_Producto)
        .Select(g => new
        {
            Producto = g.Key,
            CantidadVendida = g.Sum(x => x.Cantidad_Vendida),
            TotalVentas = g.Sum(x => x.Total_Venta)
        })
        .OrderByDescending(x => x.CantidadVendida)
        .Take(limite)
        .ToListAsync();

    return Results.Ok(ranking);
});

app.MapGet("/api/ventas/top-clientes", async (int limite, EtlDbContext db) =>
{
    var topClientes = await db.Fact_Vent
        .GroupBy(v => v.Cliente!.Nombre_Cliente)
        .Select(g => new
        {
            Cliente = g.Key,
            TotalCompras = g.Count(),
            TotalGastado = g.Sum(x => x.Total_Venta)
        })
        .OrderByDescending(x => x.TotalGastado)
        .Take(limite)
        .ToListAsync();

    return Results.Ok(topClientes);
});

app.MapGet("/api/ventas/mensual", async (int año, EtlDbContext db) =>
{
    var totales = await db.Fact_Vent
        .Where(v => v.Tiempo!.Año == año)
        .GroupBy(v => new { v.Tiempo!.Mes, v.Tiempo!.Nombre_Mes })
        .Select(g => new
        {
            Mes = g.Key.Nombre_Mes,
            Total = g.Sum(x => x.Total_Venta)
        })
        .OrderBy(g => g.Mes)
        .ToListAsync();

    return Results.Ok(totales);
});

#endregion

app.Run();
