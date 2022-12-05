namespace EventsAreObsolete.Api;

public class TickerBackgroundService : BackgroundService
{
    private readonly TickerService tickerService;

    public TickerBackgroundService(TickerService tickerService)
    {
        this.tickerService = tickerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            tickerService.OnTick(TimeOnly.FromDateTime(DateTime.Now));
            await Task.Delay(1_000, stoppingToken);
        }
    }
}