using GettingStarted;
using MassTransit;

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<MessageConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddMassTransitHostedService(waitUntilStarted: true);

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
