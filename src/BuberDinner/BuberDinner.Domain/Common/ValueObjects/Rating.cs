using BuberDinner.Domain.Dinner.ValueObjects;
using BuberDinner.Domain.Models;

namespace BuberDinner.Domain.Common.ValueObjects;

public sealed class Rating : ValueObject
{
    public double Value { get; }

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