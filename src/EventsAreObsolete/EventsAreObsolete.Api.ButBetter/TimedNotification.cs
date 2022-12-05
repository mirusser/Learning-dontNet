using MediatR;

namespace EventsAreObsolete.Api.ButBetter;

public class TimedNotification : INotification
{
    public TimeOnly Time { get; }

    public TimedNotification(TimeOnly time)
    {
        Time = time;
    }
}