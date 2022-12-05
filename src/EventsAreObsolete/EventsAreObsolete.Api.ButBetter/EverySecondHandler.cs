using MediatR;

namespace EventsAreObsolete.Api.ButBetter;

public class EverySecondHandler : INotificationHandler<TimedNotification>
{
    private readonly TransientService transientService;

    public EverySecondHandler(TransientService transientService)
    {
        this.transientService = transientService;
    }

    public Task Handle(
        TimedNotification notification,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(transientService.Id);
        Console.WriteLine(notification.Time.ToLongTimeString());
        return Task.CompletedTask;
    }
}