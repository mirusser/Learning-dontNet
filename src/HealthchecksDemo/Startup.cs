using HealthchecksDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using HealthchecksDemo.Models.DataModels.Context;
using Microsoft.EntityFrameworkCore;
using HealthchecksDemo.HealthChecks;

namespace HealthchecksDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(string.Format($"{AppDomain.CurrentDomain.BaseDirectory}\\HealthchecksDemo.xml"));

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "HealthchecksDemo",
                });
            });
            #endregion
            services.AddControllers();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>()
                .AddUrlGroup(new Uri("https://duckduckgo.com"), name: "duckduckgo")
                .AddCheck<CustomHealthCheck>(name: "New Custom Check"); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HealthchecksDemo");
            });
            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            #region Healthchecks
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
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
                            Description = x.Value.Description
                        })
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            });
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
