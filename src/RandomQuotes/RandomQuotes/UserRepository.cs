using Microsoft.EntityFrameworkCore;
using RandomQuotes.Models;

namespace RandomQuotes;

public interface IUserRepository
{
    Task<User> GetFirstOrCreateAsync(CancellationToken ct = default);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<User> GetFirstOrCreateAsync(CancellationToken ct = default)
    {
        var user = await db.Users.FirstOrDefaultAsync(ct);

        if (user is null)
        {
            user = new User() { Name = "John Doe" };
            await AddAsync(user, ct);
        }

        return user;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Users
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await db.Users.AddAsync(user, ct);
        await db.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        return db.Users.AnyAsync(q => q.Id == id, ct);
    }
}