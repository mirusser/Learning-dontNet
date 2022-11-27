using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Common.Interfaces.Persistence;

public interface IBreakfastRepository
{
    void Add(Domain.Entities.Breakfast breakfast);
    Domain.Entities.Breakfast? Get(Guid id);
}