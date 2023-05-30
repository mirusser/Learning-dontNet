using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.MenuAggregate;

namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly BuberDinnerDbContext dbContext;

    public MenuRepository(BuberDinnerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(Menu menu)
    {
        dbContext.Add(menu);
        dbContext.SaveChanges();
    }

    public async Task AddAsync(Menu menu)
    {
        await dbContext.AddAsync(menu);
        await dbContext.SaveChangesAsync();
    }
}