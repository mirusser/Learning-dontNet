using BuberDinner.Domain.Models;

namespace BuberDinner.Domain.MenuAggregate.ValueObjects;

public sealed class MenuSectionId : ValueObject
{
    public Guid Value { get; }

    private MenuSectionId(Guid value)
    {
        Value = value;
    }

    public static MenuSectionId CreteUnique()
    {
        return new(Guid.NewGuid());
    }

    public static MenuSectionId Create(Guid value)
    {
        // TODO: enforce invariants
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}