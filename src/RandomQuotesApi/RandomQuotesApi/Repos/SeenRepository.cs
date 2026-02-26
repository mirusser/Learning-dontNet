using Microsoft.EntityFrameworkCore;
using RandomQuotesApi.Models;

namespace RandomQuotesApi.Repos;

public interface ISeenRepository
{
    Task<HashSet<Guid>> GetSeenQuoteIdsAsync(Guid userId, CancellationToken ct = default);
    Task MarkSeenAsync(Guid userId, Guid quoteId, CancellationToken ct = default);
    Task ClearSeenAsync(Guid userId, CancellationToken ct = default);
}

public class SeenRepository(AppDbContext db) : ISeenRepository
{
    public async Task<HashSet<Guid>> GetSeenQuoteIdsAsync(Guid userId, CancellationToken ct = default)
    {
        return await db.SeenQuotes
                .Where(s => s.UserId == userId)
                .Select(s => s.QuoteId)
                .ToHashSetAsync(ct);
    }

    public async Task MarkSeenAsync(Guid userId, Guid quoteId, CancellationToken ct = default)
    {
        var exists = await db.SeenQuotes
            .AnyAsync(s => s.UserId == userId && s.QuoteId == quoteId, ct);

        if (!exists)
        {
            Seen seen = new()
            {
                UserId = userId,
                QuoteId = quoteId,
                Date = DateTime.UtcNow
            };
            
            await db.SeenQuotes.AddAsync(seen, ct);
            await db.SaveChangesAsync(ct);
        }
    }

    public async Task ClearSeenAsync(Guid userId, CancellationToken ct = default)
    {
        var items = await db.SeenQuotes
            .Where(s => s.UserId == userId)
            .ToListAsync(ct);

        if (items.Count == 0) return;

        db.SeenQuotes.RemoveRange(items);
        await db.SaveChangesAsync(ct);
    }
}
