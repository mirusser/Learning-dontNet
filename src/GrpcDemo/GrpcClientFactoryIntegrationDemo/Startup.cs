using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GrpcClientFactoryIntegrationDemo.Services;
using GrpcServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcClientFactoryIntegrationDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Configure your services here
        public async Task ConfigureServices(IServiceCollection services)
        {
            services.AddGrpcClient<Greeter.GreeterClient>("Greeter", o =>
            {
                o.Address = new Uri("https://localhost:5001");
            })
            //.EnableCallContextPropagation() //can't use is here for some reason, TODO: figure it out why
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                return handler;
            });

            services.AddScoped<IDemoService, DemoService>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //Get Service and call method
            var service = serviceProvider.GetService<IDemoService>();
            await service.StartAsync(CancellationToken.None);
        }
    }
}
