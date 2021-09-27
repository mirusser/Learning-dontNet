using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MassTransit;
using MassTransit.Mediator;
using UsageMediator.Consumers;
using UsageMediator;

internal sealed class ConsoleHostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;

    public ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    IMediator mediator = Bus.Factory.CreateMediator(cfg =>
                    {
                        cfg.Consumer<OrderStatusConsumer>();
                        cfg.Consumer<SubmitOrderConsumer>();
                    });

                    Guid orderId = NewId.NextGuid();

                    await mediator.Send<SubmitOrder>(new { OrderId = orderId });

                    var client = mediator.CreateRequestClient<GetOrderStatus>();

                    var response = await client.GetResponse<OrderStatus>(new { OrderId = orderId });

                    Console.WriteLine("Order Status: {0}", response.Message.Status);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception!");
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}