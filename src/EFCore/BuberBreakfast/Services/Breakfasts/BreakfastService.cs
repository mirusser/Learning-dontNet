using BuberBreakfast.Models;
using BuberBreakfast.Persistence;
using BuberBreakfast.ServiceErrors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace BuberBreakfast.Services.Breakfasts;

public class BreakfastService : IBreakfastService
{
    //private static readonly Dictionary<Guid, Breakfast> _breakfasts = new();
    private readonly BuberBreakfastDbContext dbContext;

    public BreakfastService(BuberBreakfastDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public ErrorOr<Created> CreateBreakfast(Breakfast breakfast)
    {
        dbContext.Add(breakfast);
        dbContext.SaveChanges();

        return Result.Created;
    }

    public ErrorOr<Deleted> DeleteBreakfast(Guid id)
    {
        var breakfast = dbContext.Breakfasts.Find(id);

        if (breakfast is null)
        {
            return Errors.Breakfast.NotFound;
        }

        dbContext.Remove(breakfast);
        dbContext.SaveChanges();

        return Result.Deleted;
    }

    public ErrorOr<Breakfast> GetBreakfast(Guid id)
    {
        if (dbContext.Breakfasts.Find(id) is Breakfast breakfast)
        {
            return breakfast;
        }

        return Errors.Breakfast.NotFound;
    }

    public ErrorOr<UpsertedBreakfast> UpsertBreakfast(Breakfast breakfast)
    {
        var isNewlyCreated = !dbContext.Breakfasts
            .Any(b => b.Id == breakfast.Id);

        if (isNewlyCreated)
        {
            dbContext.Breakfasts.Add(breakfast);
        }
        else
        {
            dbContext.Breakfasts.Update(breakfast);
        }

        dbContext.SaveChanges();

        return new UpsertedBreakfast(isNewlyCreated);
    }
}
