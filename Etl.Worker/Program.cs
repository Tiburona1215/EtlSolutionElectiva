using Etl.Core.Innterface;
using Etl.Core.Models;
using Etl.Data;
using Etl.Infrastructure.Estractors;
using Etl.Worker;
using Etl.Worker.Loaders;
using Etl.Worker.Transformers;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<EtlDbContext>(opt =>
            opt.UseSqlServer("Server=localhost;Database=ETL_DB;Trusted_Connection=True;TrustServerCertificate=True"));

        string csvPath = Path.Combine(AppContext.BaseDirectory, "ventas.csv");
        services.AddSingleton<IExtractor<RawVentaDto>>(sp => new CsvExtractor<RawVentaDto>(csvPath));
        services.AddTransient<ITransformer<RawVentaDto, FactVent>, FactTransformer>();
        services.AddTransient<ILoader<FactVent>, EfLoader>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();