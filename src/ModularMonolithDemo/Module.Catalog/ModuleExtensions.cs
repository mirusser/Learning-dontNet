using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Catalog.Core.Extensions;
using Module.Catalog.Infrastructure.Extensions;

namespace Module.Catalog;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, string connectionString)
    {
        services
            .AddCatalogCore()
            .AddCatalogInfrastructure(connectionString);
        return services;
    }
}
