using System;
using System.Threading.Tasks;
using GrpcClientFactoryIntegrationDemo.Extensions;
using GrpcClientFactoryIntegrationDemo.Services;
using Microsoft.Extensions.Hosting;

namespace GrpcClientFactoryIntegrationDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .Build()
                .RunAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}
