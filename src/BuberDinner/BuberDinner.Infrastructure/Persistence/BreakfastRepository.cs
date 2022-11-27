using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Infrastructure.Persistence;

public class BreakfastRepository : IBreakfastRepository
{
    private static readonly List<Breakfast> breakfasts = new();

    public void Add(Breakfast breakfast)
    {
        breakfasts.Add(breakfast);
    }

    public Breakfast? Get(Guid id)
    {
        return breakfasts.Find(b => b.Id == id);
    }
}