﻿using Domain;
using Domain.Consts;
using Nest;

namespace Ingest;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<StockIngestWorker>();
                services.AddHostedService<ReindexWorker>();
                services.AddSingleton<IElasticClient>(sp =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();
                    var settings = new ConnectionSettings() // default
                        .DefaultIndex(Indexes.Default)
                        .DefaultMappingFor<StockData>(i => i.IndexName(Indexes.StockDemoV1))
                        .EnableDebugMode()
                        .EnableApiVersioningHeader(); //https://github.com/elastic/elasticsearch-net/issues/6154

                    //example using cloudId:
                    //var settings = new ConnectionSettings(
                    //    config["cloudId"],
                    //    new BasicAuthenticationCredentials("elastic", config["password"]));

                    return new ElasticClient(settings);
                });
                services.AddSingleton<StockDataReader>();
            });
}