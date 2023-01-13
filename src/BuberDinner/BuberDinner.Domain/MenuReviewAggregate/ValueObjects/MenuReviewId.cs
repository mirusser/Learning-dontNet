using BuberDinner.Domain.Models;

namespace BuberDinner.Domain.MenuReviewAggregate.ValueObjects;

public sealed class MenuReviewId : ValueObject
{
    public Guid Value { get; private set; }

    private MenuReviewId() { }
    private MenuReviewId(Guid value)
    {
        Value = value;
    }

    public static MenuReviewId CreteUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}