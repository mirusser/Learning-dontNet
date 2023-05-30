namespace BuberDinner.Domain.Common.Models;

public abstract class AggregateRoot<TId, TIdType> : Entity<TId>
    where TId : AggregateRootId<TIdType>
{
    public new AggregateRootId<TIdType> Id { get; protected set; }

    protected AggregateRoot()
    { }

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    // Fix (hack) for mapster mapping configuration
    public AggregateRootId<TIdType> GetId() => this.Id;
}