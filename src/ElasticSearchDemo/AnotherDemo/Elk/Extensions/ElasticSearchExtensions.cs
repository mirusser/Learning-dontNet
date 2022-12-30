using System;
using Elk.Models;
using Nest;

namespace Elk.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IElasticClient>(_ =>
        {
            var url = configuration["ElkConfiguration:Uri"];
            var defaultIndex = configuration["ElkConfiguration:Index"];

            //var config = sp.GetRequiredService<IConfiguration>();
            var settings = new ConnectionSettings(new Uri(url))
                .PrettyJson()
                .DefaultIndex(defaultIndex)
                .AddDefaultMappings(defaultIndex)
                .EnableDebugMode()
                .EnableApiVersioningHeader(); //https://github.com/elastic/elasticsearch-net/issues/6154

            var client =  new ElasticClient(settings);

            CreateIndex(client, defaultIndex);

            return client;
        });
    }

    private static ConnectionSettings AddDefaultMappings(
        this ConnectionSettings settings, 
        string defaultIndex)
    {
        settings
            .DefaultMappingFor<Product>(p => p
                .Ignore(x=> x.Price)
                .Ignore(x=> x.Id)
                .Ignore(x=> x.Quantity)
                .IndexName(defaultIndex));

        return settings;
    }

    private static void CreateIndex(IElasticClient client, string indexName)
    {
        client.Indices.Create(indexName, i => i.Map<Product>(x => x.AutoMap()));
    }
}
