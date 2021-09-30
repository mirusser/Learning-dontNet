using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommService.Providers;
using EcommService.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MassTransit;
using EcommService.Listeners;

namespace EcommService
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
            services.Configure<ConnectionStrings>(Configuration.GetSection(nameof(ConnectionStrings)));

            services.AddHealthChecks();
            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(2);
                options.Predicate = (check) => check.Tags.Contains("ready");
            });

            services.AddMassTransit(config =>
            {
                config.AddConsumer<OrderCreatedListener>();
                config.SetKebabCaseEndpointNameFormatter();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("rabbitmq://rabbitmqsaga");
                    cfg.ConfigureEndpoints(ctx);
                });
            });
            services.AddMassTransitHostedService(waitUntilStarted: true);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EcommService", Version = "v1" });
            });

            services.AddScoped<IInventoryProvider, InventoryProvider>();
            services.AddScoped<IInventoryUpdatorProvider, InventoryUpdatorProvider>();
            services.AddScoped<IOrderDetailsProvider, OrderDetailsProvider>();
            services.AddScoped<IProductProvider, ProductProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EcommService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}