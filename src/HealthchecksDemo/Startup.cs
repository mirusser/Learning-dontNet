using System;
using CarDealership.HealthChecks;
using HealthchecksDemo.HealthChecks;
using HealthchecksDemo.HealthChecks.Middlewares;
using HealthchecksDemo.Models.DataModels.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace HealthchecksDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();

            services.AddServiceHealthChecks(Configuration);

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "HealthchecksDemo",
                });
            });

            #endregion Swagger

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HealthchecksDemo");
            });

            #endregion Swagger

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            #region Healthchecks

            app.UseCustomHealthCheckReady();
            app.UseCustomFullHealthCheck();
            app.UseServiceHealthCheckUI();

            #endregion Healthchecks

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}