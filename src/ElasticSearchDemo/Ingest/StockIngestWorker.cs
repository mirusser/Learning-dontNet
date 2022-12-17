using System.Runtime.ExceptionServices;
using Domain.Consts;
using Nest;

namespace Ingest;

public class StockIngestWorker : BackgroundService
{
    private readonly ILogger<StockIngestWorker> logger;
    private readonly IElasticClient client;
    private readonly StockDataReader stockDataReader;
    private readonly IHostApplicationLifetime applicationLifetime;

    public StockIngestWorker(
        ILogger<StockIngestWorker> logger,
        IElasticClient client,
        StockDataReader stockDataReader,
        IHostApplicationLifetime applicationLifetime)
    {
        this.logger = logger;
        this.client = client;
        this.stockDataReader = stockDataReader;
        this.applicationLifetime = applicationLifetime;
    }

    //in case of errors look up: https://stackoverflow.com/a/63390110/11613167
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bulkAll = client.BulkAll<Domain.StockData>(stockDataReader.StockData(),
                b => b.Index(Domain.Consts.Indexes.StockDemoV1)
                    .BackOffRetries(2)
                    .BackOffTime(TimeSpan.FromSeconds(30))
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .Size(1000),
                cancellationToken: stoppingToken
            );

        var waitHandle = new CountdownEvent(1);

        ExceptionDispatchInfo? captureInfo = null;

        var subscription = bulkAll.Subscribe(new BulkAllObserver(
            onNext: _ => logger.LogInformation("Data indexed"),
            onError: e =>
            {
                captureInfo = ExceptionDispatchInfo.Capture(e);
                waitHandle.Signal();
            },
            onCompleted: () => waitHandle.Signal()
        ));

        waitHandle.Wait(TimeSpan.FromMinutes(30), stoppingToken);

        if (captureInfo is not null
            && captureInfo.SourceException is not OperationCanceledException)
        {
            captureInfo?.Throw();
        }

        await client.Indices.PutAliasAsync(
            Domain.Consts.Indexes.StockDemoV1,
            Domain.Consts.Indexes.Aliases.StockDemo,
            ct: stoppingToken);
    }
}