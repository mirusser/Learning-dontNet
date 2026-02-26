using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RandomQuotes.Models;

namespace RandomQuotes;

public class FavoritesRepository1
{
    private readonly List<Favorite> allFavorites = new();
    private readonly SemaphoreSlim mutex = new(1, 1);

    private const string fileName = "Favorites.json";
    private readonly string path = Path.Combine(AppContext.BaseDirectory, fileName);

    // Factory so you can do async initialization cleanly
    public static async Task<FavoritesRepository1> CreateAsync()
    {
        var repo = new FavoritesRepository1();
        await repo.LoadFavoritesAsync();
        return repo;
    }

    // private ctor so others must use CreateAsync()
    private FavoritesRepository1()
    {
    }

    public async Task<IReadOnlyList<Favorite>> GetAllAsync()
    {
        // You *could* skip the lock for read-only access,
        // but then callers must ensure no concurrent writers.
        await mutex.WaitAsync();
        try
        {
            // Return a copy to avoid external mutation
            return allFavorites.ToList();
        }
        finally
        {
            mutex.Release();
        }
    }

    private async Task LoadFavoritesAsync()
    {
        await mutex.WaitAsync();
        try
        {
            if (!File.Exists(path))
            {
                await File.WriteAllTextAsync(path, "[]");
            }

            var json = await File.ReadAllTextAsync(path);

            var deserialized = JsonSerializer.Deserialize<List<Favorite>>(json);
            if (deserialized is null)
                throw new InvalidOperationException($"{fileName} deserialized to null.");

            allFavorites.Clear();
            allFavorites.AddRange(deserialized);
        }
        finally
        {
            mutex.Release();
        }
    }

    private async Task SaveFavoritesAsync()
    {
        // Assumes caller already holds the mutex.
        var serialized = JsonSerializer.Serialize(allFavorites);
        await File.WriteAllTextAsync(path, serialized);
    }

    public async Task ClearFavoritesAsync()
    {
        await mutex.WaitAsync();
        try
        {
            allFavorites.Clear();
            await SaveFavoritesAsync();
        }
        finally
        {
            mutex.Release();
        }
    }

    public async Task<bool> TryAddAsync(Guid userId, Guid quoteId, CancellationToken ct = default)
    {
        await mutex.WaitAsync(ct);
        try
        {
            if (allFavorites.Any(f => f.QuoteId == quoteId && f.UserId == userId))
                return false;

            var favorite = new Favorite() { UserId = userId, QuoteId = quoteId };
            allFavorites.Add(favorite);

            await SaveFavoritesAsync();
            return true;
        }
        finally
        {
            mutex.Release();
        }
    }

    public async Task<bool> TryRemoveAsync(Guid userId, Guid quoteId, CancellationToken ct = default)
    {
        await mutex.WaitAsync(ct);
        try
        {
            var removed = allFavorites.RemoveAll(f => f.QuoteId == quoteId && f.UserId == userId);

            if (removed > 0)
            {
                await SaveFavoritesAsync();
                return true;
            }

            return false;
        }
        finally
        {
            mutex.Release();
        }
    }

    public async Task<Favorite?> TryGetAsync(Guid userId, Guid quoteId)
    {
        await mutex.WaitAsync();
        try
        {
            return allFavorites
                .FirstOrDefault(f => f.UserId == userId && f.QuoteId == quoteId);
        }
        finally
        {
            mutex.Release();
        }
    }

    public async Task<int> HowManyUsersAddedAsFavoriteAsync(Guid quoteId)
    {
        await mutex.WaitAsync();
        try
        {
            return allFavorites.Count(f => f.QuoteId == quoteId);
        }
        finally
        {
            mutex.Release();
        }
    }
}

public interface IFavoritesRepository
{
    Task<IReadOnlyList<Favorite>> GetAllAsync(CancellationToken ct = default);
    Task<bool> TryAddAsync(Guid userId, Guid quoteId, CancellationToken ct = default);
    Task<bool> TryRemoveAsync(Guid userId, Guid quoteId, CancellationToken ct = default);
    Task ClearAsync(Guid userId, CancellationToken ct = default);
    Task<Favorite?> TryGetAsync(Guid userId, Guid quoteId, CancellationToken ct = default);
    Task<int> HowManyUsersAddedAsFavoriteAsync(Guid quoteId, CancellationToken ct = default);
}

public class FavoritesRepository : IFavoritesRepository
{
    private readonly AppDbContext db;

    public FavoritesRepository(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<IReadOnlyList<Favorite>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Favorites
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