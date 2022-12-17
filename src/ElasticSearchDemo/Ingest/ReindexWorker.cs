using Domain;
using Domain.Consts;
using Nest;

namespace Ingest;

public class ReindexWorker : BackgroundService
{
    private readonly ILogger<ReindexWorker> logger;
    private readonly IElasticClient elasticClient;
    private readonly IHostApplicationLifetime applicationLifetime;

    public ReindexWorker(
        ILogger<ReindexWorker> logger,
        IElasticClient elasticClient,
        IHostApplicationLifetime applicationLifetime)
    {
        this.logger = logger;
        this.elasticClient = elasticClient;
        this.applicationLifetime = applicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken = default)
    {
        var response = await elasticClient.Indices.ExistsAsync(
            Indexes.StockDemoV2,
            ct: stoppingToken);

        if (response.Exists)
        {
            await elasticClient.Indices.DeleteAsync(
                Indexes.StockDemoV2,
                ct: stoppingToken);
        }

        var newIndexResponse = await elasticClient.Indices.CreateAsync(
            Indexes.StockDemoV2, 
            i =>
                i.Map(m => m
                    .AutoMap<StockData>()
                    .Properties<StockData>(p => p
                        .Keyword(k => k.Name(f => f.Name)))), 
            stoppingToken);

        if (newIndexResponse.IsValid)
        {
            logger.LogInformation("Create new index");

            var reindex = await elasticClient
                    .ReindexOnServerAsync(
                        r => r
                            .Source(s => s.Index(Indexes.StockDemoV1))
                            .Destination(d => d.Index(Indexes.StockDemoV2))
                            .WaitForCompletion(false),
                        stoppingToken);
            var taskId = reindex.Task;
            var taskResponse = await elasticClient.Tasks.GetTaskAsync(taskId, ct: stoppingToken);

            while (!taskResponse.Completed)
            {
                logger.LogInformation("Waiting for 5 seconds");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                taskResponse = await elasticClient.Tasks.GetTaskAsync(taskId, ct: stoppingToken);
            }

            logger.LogInformation("Reindex completed");
        }
    }
}