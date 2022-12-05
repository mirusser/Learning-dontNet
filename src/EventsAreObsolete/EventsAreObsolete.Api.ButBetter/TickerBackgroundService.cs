using MediatR;

namespace EventsAreObsolete.Api.ButBetter;

public class TickerBackgroundService : BackgroundService
{
    private readonly IMediator mediator;

    public TickerBackgroundService(IMediator mediator)
    {
        this.mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var timeNow = TimeOnly.FromDateTime(DateTime.Now);
            await mediator.Publish(new TimedNotification(timeNow), stoppingToken);

            await Task.Delay(1_000, stoppingToken);
        }
    }
}