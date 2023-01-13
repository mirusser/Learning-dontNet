using BuberDinner.Domain.Models;

namespace BuberDinner.Domain.MenuAggregate.ValueObjects;

public sealed class MenuId : ValueObject
{
    public Guid Value { get; }

    private MenuId(Guid value)
    {
        Value = value;
    }

    public static MenuId CreteUnique()
    {
        // TODO: enforce invariants
        return new(Guid.NewGuid());
    }

    public static MenuId Create(Guid value)
    {
        // TODO: enforce invariants
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}