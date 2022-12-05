using MediatR;

namespace EventsAreObsolete.Api.ButBetter;

public class EveryFiveSecondHandler : INotificationHandler<TimedNotification>
{
    public Task Handle(
        TimedNotification notification,
        CancellationToken cancellationToken)
    {
        if (notification.Time.Second % 5 == 0)
        {
            Console.WriteLine(notification.Time.ToLongTimeString());
        }

        return Task.CompletedTask;
    }
}