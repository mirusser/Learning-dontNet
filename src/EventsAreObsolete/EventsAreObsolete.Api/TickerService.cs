namespace EventsAreObsolete.Api;

public class TickerService
{
    public EventHandler<TickerEventArgs> Ticked;
    private readonly TransientService transientService;

    public TickerService(TransientService transientService)
    {
        this.transientService = transientService;
        Ticked += OnEverySecond;
        Ticked += OnEveryFiveSecond;
    }

    private void OnEverySecond(object? sender, TickerEventArgs args)
    {
        Console.WriteLine(args.Time.ToLongTimeString());
        Console.WriteLine(transientService.Id);
    }

    private void OnEveryFiveSecond(object? sender, TickerEventArgs args)
    {
        if (args.Time.Second % 5 == 0)
        {
            Console.WriteLine(args.Time.ToLongTimeString());
        }
    }

    public void OnTick(TimeOnly time)
    {
        Ticked?.Invoke(this, new(time));
    }
}

public class TickerEventArgs
{
    public TimeOnly Time { get; }

    public TickerEventArgs(TimeOnly time)
    {
        Time = time;
    }
}