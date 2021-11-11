using System;
using CarDealership.HealthChecks.Tags;
using HealthchecksDemo.HealthChecks.Custom;
using HealthchecksDemo.Models.DataModels.Context;
using HealthchecksDemo.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CarDealership.HealthChecks
{
    public static class HealthCheckRegistration
    {
        public static void AddServiceHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(
                    name: "Database health check",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { nameof(HealthChecksTags.Database) })
                .AddUrlGroup(
                    new Uri("http://clients1.google.com/generate_204"),
                    name: "Serivce: Example of external service health check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { nameof(HealthChecksTags.ExternalService) })
                .AddMemoryHealthCheck(
                    name: "Memory health check",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { nameof(HealthChecksTags.Memory) });

            services.AddHealthChecksUI(opt =>
            {
                HealthChecksSettings healthChecksSettings = new();
                configuration.GetSection(nameof(HealthChecksSettings)).Bind(healthChecksSettings);

                opt.SetEvaluationTimeInSeconds(healthChecksSettings.UIConfiguration.EvaluationTimeInSeconds);
                opt.MaximumHistoryEntriesPerEndpoint(healthChecksSettings.UIConfiguration.MaxNumberOfEntriesPerEndpoint);
                opt.SetApiMaxActiveRequests(healthChecksSettings.UIConfiguration.ApiRequestsConcurrency);

                opt.AddHealthCheckEndpoint("All health checks", "/health-ui");
            })
            .AddInMemoryStorage();
        }
    }
}