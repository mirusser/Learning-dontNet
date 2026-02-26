using RandomQuotesApi.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace RandomQuotesApi.Repos;

public interface IQuoteRepository
{
    Task<Quote?> GetRandomUnseenAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<Quote>> GetAllAsync(CancellationToken ct = default);
    Task<Quote?> GetAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Quote quote, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);

    Task<(IReadOnlyList<Quote> Quotes, int TotalCount)> GetPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);
}

public class QuoteRepository(AppDbContext db) : IQuoteRepository
{
    // NOTE: This query (ORDER BY RANDOM()) is simple and perfectly fine for our current scale.
    // If we ever need to handle large datasets or high traffic, revisit this: random ordering
    // forces a full table scan + sort. In that case we'd switch to a more scalable random-pick
    // strategy (e.g., precomputed IDs, skip/take offsets, or reservoir sampling).
    // Future optimization: use a Bloom filter or other probabilistic cache to avoid
    // DB "seen" checks — helps at very large scale but unnecessary for now.
    public async Task<Quote?> GetRandomUnseenAsync(Guid userId, CancellationToken ct = default)
    {
        return await db.Quotes
            .Where(q => !db.SeenQuotes.Any(s => s.UserId == userId && s.QuoteId == q.Id))
            .OrderBy(q => EF.Functions.Random())
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyList<Quote>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Quotes
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Quote?> GetAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Quotes
            .FirstOrDefaultAsync(q => q.Id == id, ct);
    }

    public async Task AddAsync(Quote quote, CancellationToken ct = default)
    {
        await db.Quotes.AddAsync(quote, ct);
        await db.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
        return db.Quotes.AnyAsync(q => q.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Quote> Quotes, int TotalCount)> GetPageAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var query = db.Quotes
            .AsNoTracking()
            .OrderBy(q => q.Author)
            .ThenBy(q => q.Id);

        var totalCount = await query.CountAsync(ct);

        var quotes = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (quotes, totalCount);
    }
}