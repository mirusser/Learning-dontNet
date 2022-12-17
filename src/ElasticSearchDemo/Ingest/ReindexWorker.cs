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
        await DeleteIndexIfExists();

        var newIndexResponse = await CreateIndex();

        if (newIndexResponse?.IsValid == true)
        {
            await Reindex();
        }

        applicationLifetime.StopApplication();

        async Task DeleteIndexIfExists()
        {
            var response = await elasticClient.Indices.ExistsAsync(
                Indexes.StockDemoV2,
                ct: stoppingToken);

            if (response.Exists)
            {
                logger.LogInformation(
                    "Index: {stockDemoV2} exits, deleting", 
                    Indexes.StockDemoV2);

                await elasticClient.Indices.DeleteAsync(
                    Indexes.StockDemoV2,
                    ct: stoppingToken);
            }
        }

        async Task<CreateIndexResponse?> CreateIndex()
        {
            logger.LogInformation("Create new index");

            var newIndexResponse = await elasticClient.Indices.CreateAsync(
                Indexes.StockDemoV2,
                i =>
                    i.Map(m => m
                        .AutoMap<StockData>()
                        .Properties<StockData>(p => p
                            .Keyword(k => k.Name(f => f.Name)))),
                stoppingToken);

            return newIndexResponse;
        }

        async Task Reindex()
        {
            logger.LogInformation("Reindex");

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

            await elasticClient.Indices.BulkAliasAsync(aliases => aliases
                .Remove(a => a.Alias(Indexes.Aliases.StockDemo).Index("*"))
                .Add(a => a.Alias(Indexes.Aliases.StockDemo).Index(Indexes.StockDemoV2)),
                stoppingToken);

            logger.LogInformation("Alias updated");
        }
    }
}