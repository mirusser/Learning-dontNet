using MassTransit;
using Shared.Models;

namespace Listener.Consumers;

public class EventConsumer : IConsumer<ValueEntered>
{
    private readonly ILogger<EventConsumer> _logger;

    public EventConsumer(ILogger<EventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ValueEntered> context)
    {
        _logger.LogInformation("Got message: {message}", context.Message.Value);
    }
}
