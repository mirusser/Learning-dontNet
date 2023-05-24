﻿using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.HostAggregate.ValueObjects;

public sealed class HostId : AggregateRootId<Guid>
{
    public override Guid Value { get; protected set; }

    private HostId()
    {
    }

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

    public static HostId Create(Guid value)
    {
        return new(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}