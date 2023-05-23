﻿using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.MenuAggregate.ValueObjects;

public sealed class MenuItemId : ValueObject
{
    public Guid Value { get; private set; }

    private MenuItemId()
    { }

    private MenuItemId(Guid value)
    {
        Value = value;
    }

    public static MenuItemId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static MenuItemId Create(Guid value)
    {
        // TODO: enforce invariants
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}