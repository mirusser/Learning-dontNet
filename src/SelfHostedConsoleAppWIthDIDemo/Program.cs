using Microsoft.Extensions.Hosting;
using SelfHostedConsoleAppWIthDIDemo;
using SelfHostedConsoleAppWIthDIDemo.Extensions;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseStartup<Startup>();

var host = CreateHostBuilder(args).Build();
await host.RunAsync();
