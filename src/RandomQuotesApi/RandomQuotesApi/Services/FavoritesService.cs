using RandomQuotesApi.Models.DTOs;
using RandomQuotesApi.Repos;
using RandomQuotesApi.Services.Contracts;

namespace RandomQuotesApi.Services;

public interface IFavoritesService
{
    Task<IReadOnlyList<FavoriteQuoteDto>> GetAllForUserAsync(Guid userId, CancellationToken ct = default);
    Task<bool> AddAsync(AddFavoriteRequest request, CancellationToken ct = default);
    Task RemoveAllForUserAsync(Guid userId, CancellationToken ct = default);
    Task<bool> RemoveAsync(Guid userId, Guid quoteId, CancellationToken ct = default);
}

public class FavoritesService(
    IFavoritesRepository favoritesRepository,
    ICacheService cache) : IFavoritesService
{
    private const string FavoritesCachePrefix = "favorites:user:";

    private static string CacheKeyForUser(Guid userId)
        => $"{FavoritesCachePrefix}{userId}";

    public async Task<IReadOnlyList<FavoriteQuoteDto>> GetAllForUserAsync(
        Guid userId,
        CancellationToken ct = default)
    {
        var cacheKey = CacheKeyForUser(userId);

        // Cache favorites as DTOs, not EF entities
        return await cache.GetOrCreateAsync(
            cacheKey,
            async _ =>
            {
                var favored = await favoritesRepository.GetAllForUserAsync(userId, ct);

                return favored
                    .Select(f => new FavoriteQuoteDto(
                        f.Quote!.Id,
                        f.Quote.Text,
                        f.Quote.Author,
                        f.UserId))
                    .ToList();
            },
            ttl: TimeSpan.FromMinutes(5),
            ct: ct);
    }

    public async Task<bool> AddAsync(AddFavoriteRequest request, CancellationToken ct = default)
    {
        var added = await favoritesRepository.TryAddAsync(request.UserId, request.QuoteId, ct);

        if (added)
        {
            // Invalidate cache for that user
            await cache.RemoveAsync(CacheKeyForUser(request.UserId), ct);
        }

        return added;
    }

    public async Task RemoveAllForUserAsync(Guid userId, CancellationToken ct = default)
    {
        await favoritesRepository.ClearAsync(userId, ct);

        await cache.RemoveAsync(CacheKeyForUser(userId), ct);
    }

    public async Task<bool> RemoveAsync(Guid userId, Guid quoteId, CancellationToken ct = default)
    {
        var removed = await favoritesRepository.TryRemoveAsync(userId, quoteId, ct);

        if (removed)
        {
            await cache.RemoveAsync(CacheKeyForUser(userId), ct);
        }

        return removed;
    }
}
