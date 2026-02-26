using Microsoft.EntityFrameworkCore;
using RandomQuotesApi.Models;

namespace RandomQuotesApi.Repos;

public interface IFavoritesRepository
{
    Task<IReadOnlyList<Favorite>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Favorite>> GetAllForUserAsync(Guid userId, CancellationToken ct = default);
    Task<bool> TryAddAsync(Guid userId, Guid quoteId, CancellationToken ct = default);
    Task<bool> TryRemoveAsync(Guid userId, Guid quoteId, CancellationToken ct = default);
    Task ClearAsync(Guid userId, CancellationToken ct = default);
    Task<Favorite?> TryGetAsync(Guid userId, Guid quoteId, CancellationToken ct = default);
    Task<int> HowManyUsersAddedAsFavoriteAsync(Guid quoteId, CancellationToken ct = default);
}

public class FavoritesRepository(AppDbContext db) : IFavoritesRepository
{
    public async Task<IReadOnlyList<Favorite>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Favorites
            .AsNoTracking()
            .ToListAsync(ct);
    }
    
    public async Task<IReadOnlyList<Favorite>> GetAllForUserAsync(Guid userId, CancellationToken ct = default)
    {
        return await db.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Quote)
            .AsNoTracking()
            .ToListAsync(ct);
    }
    
    public async Task<bool> TryAddAsync(Guid userId, Guid quoteId, CancellationToken ct = default)
    {
        var exists = await db.Favorites.AnyAsync(
            f => f.UserId == userId && f.QuoteId == quoteId, ct);

        if (exists)
        {
            return false;
        }

        var favorite = new Favorite { UserId = userId, QuoteId = quoteId };
        db.Favorites.Add(favorite);
        await db.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> TryRemoveAsync(Guid userId, Guid quoteId, CancellationToken ct = default)
    {
        var favorite = await db.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.QuoteId == quoteId, ct);

        if (favorite is null)
        {
            return false;
        }

        db.Favorites.Remove(favorite);
        await db.SaveChangesAsync(ct);
        return true;
    }

    public async Task ClearAsync(Guid userId, CancellationToken ct = default)
    {
        var favorites = await db.Favorites
            .Where(f => f.UserId == userId)
            .ToListAsync(ct);
        
        db.Favorites.RemoveRange(favorites);
        await db.SaveChangesAsync(ct);
    }
    

    public Task<Favorite?> TryGetAsync(Guid userId, Guid quoteId, CancellationToken ct = default)
    {
        return db.Favorites
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.UserId == userId && f.QuoteId == quoteId, ct);
    }

    public Task<int> HowManyUsersAddedAsFavoriteAsync(Guid quoteId, CancellationToken ct = default)
    {
        return db.Favorites.CountAsync(f => f.QuoteId == quoteId, ct);
    }
}