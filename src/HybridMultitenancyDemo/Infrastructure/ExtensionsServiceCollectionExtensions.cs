using System.Linq;
using Core.Settings;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ExtensionsServiceCollectionExtensions
    {
        public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            TenantSettings tenantSettings = new();
            configuration.GetSection(nameof(TenantSettings)).Bind(tenantSettings);

            var defaultConnectionString = tenantSettings?.DefaultConfiguration.ConnectionString;
            var defaultDbProvider = tenantSettings?.DefaultConfiguration.DbProvider;

            if (defaultDbProvider == "postgres")
            {
                services.AddDbContext<ApplicationDbContext>(m => m.UseNpgsql(b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            foreach (var tenant in tenantSettings.Tenants)
            {
                string connectionString = !string.IsNullOrEmpty(tenant.ConnectionString) ? tenant.ConnectionString : defaultConnectionString;
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.SetConnectionString(connectionString);

                if (dbContext.Database.GetMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }

            return services;
        }

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);

            return options;
        }
    }
}