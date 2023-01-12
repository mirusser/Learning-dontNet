using BuberDinner.Domain.Models;

namespace BuberDinner.Domain.HostAggregate.ValueObjects;

public sealed class HostId : ValueObject
{
    public Guid Value { get; }

    private HostId(Guid value)
    {
        Value = value;
    }

    public static HostId CreteUnique()
    {
        return new(Guid.NewGuid());
    }

    public static HostId Create(string value)
    {
        if (Guid.TryParse(value, out var id))
        {
            return new(id);
        }

        throw new Exception($"Couldn't parse value: {value} to Guid");
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}