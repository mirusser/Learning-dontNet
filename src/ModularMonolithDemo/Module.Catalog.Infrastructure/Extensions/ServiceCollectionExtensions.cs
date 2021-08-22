using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module.Catalog.Core.Abstractions;
using Module.Catalog.Infrastructure.Persistence;
using Shared.Infrastructure.Extensions;

namespace Module.Catalog.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCatalogInfrastructure(this IServiceCollection services, string connectionString)
    {
        services
            .AddDatabaseContext<CatalogDbContext>(connectionString)
            .AddScoped<ICatalogDbContext>(provider => provider.GetService<CatalogDbContext>());
        return services;
    }
}

