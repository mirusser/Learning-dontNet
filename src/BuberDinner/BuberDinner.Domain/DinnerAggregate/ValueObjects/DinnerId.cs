﻿using BuberDinner.Domain.Models;

namespace BuberDinner.Domain.DinnerAggregate.ValueObjects;

public sealed class DinnerId : ValueObject
{
    public Guid Value { get; private set; }

    private DinnerId() { }
    private DinnerId(Guid value)
    {
        Value = value;
    }

    public static DinnerId CreteUnique()
    {
        return new(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}