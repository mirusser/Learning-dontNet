using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.Common.ValueObjects;

public sealed class Rating : ValueObject
{
    public double Value { get; private set; }

    private Rating()
    {
    }

    private Rating(double value)
    {
        Value = value;
    }

    public static Rating Create()
    {
        return new(default);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}