using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Jobs;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HangfireService.Settings
{
    public static class ServiceRegistration
    {
        public static void AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            //StackOverflow post on how to register hangfire with mongo:
            //https://stackoverflow.com/questions/58340247/how-to-use-hangfire-in-net-core-with-mongodb

            MongoSettings mongoSettings = new();
            configuration.GetSection(nameof(MongoSettings)).Bind(mongoSettings);

            var migrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            };

            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UseMongoStorage(mongoSettings.ConnectionString, mongoSettings.Database, new MongoStorageOptions { MigrationOptions = migrationOptions });
            });
            services.AddHangfireServer();
        }

        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {
                RabbitMQSettings rabbitMQSettings = new();
                configuration.GetSection(nameof(RabbitMQSettings)).Bind(rabbitMQSettings);

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(rabbitMQSettings.Host);
                });
            });
            services.AddMassTransitHostedService(waitUntilStarted: true);
        }
    }
}