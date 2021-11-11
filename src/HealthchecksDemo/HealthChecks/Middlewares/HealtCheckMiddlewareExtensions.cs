using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CarDealership.HealthChecks.Tags;
using HealthChecks.UI.Client;
using HealthchecksDemo.HealthChecks.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthchecksDemo.HealthChecks.Middlewares
{
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseServiceHealthChecks(this IApplicationBuilder builder)
        {
            builder.UseHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains(nameof(HealthChecksTags.Ready)),
                ResponseWriter = _responseWriter
            });

            builder.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = _responseWriter
            });

            builder.UseHealthChecks("/health-ui", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return builder;
        }

        #region Response writer Func

        private static readonly Func<HttpContext, HealthReport, Task> _responseWriter = async (context, report) =>
              {
                  context.Response.ContentType = "application/json";
                  var response = new HealthCheckReponse
                  {
                      Status = report.Status.ToString(),
                      HealthCheckDuration = report.TotalDuration,
                      HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
                      {
                          Component = x.Key,
                          Status = x.Value.Status.ToString(),
                          Description = x.Value.Description,
                          Exception = x.Value.Exception
                      })
                  };
                  await context.Response.WriteAsync(JsonSerializer.Serialize(response)).ConfigureAwait(false);
              };

        #endregion Response writer Func
    }
}