using Etl.Core.Innterface;
using Etl.Core.Models;
using Etl.Data;

namespace Etl.Worker;

public class Worker : BackgroundService
{
    private readonly IExtractor<RawVentaDto> _extractor;
    private readonly ITransformer<RawVentaDto, FactVent> _transformer;
    private readonly ILoader<FactVent> _loader;
    private readonly ILogger<Worker> _logger;

    public Worker(IExtractor<RawVentaDto> extractor,
                     ITransformer<RawVentaDto, FactVent> transformer,
                     ILoader<FactVent> loader,
                     ILogger<Worker> logger)
    {
        _extractor = extractor;
        _transformer = transformer;
        _loader = loader;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ETL iniciado...");

        var raw = await _extractor.ExtractAsync(stoppingToken);
        var fact = await _transformer.TransformAsync(raw, stoppingToken);
        await _loader.LoadAsync(fact, stoppingToken);

        _logger.LogInformation("ETL completado.");
    }
}
