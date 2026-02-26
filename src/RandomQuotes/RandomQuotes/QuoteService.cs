using RandomQuotes.Models;

namespace RandomQuotes;

public class QuoteService(
    IQuoteRepository quoteRepository,
    FavoritesRepository? favoritesRepository = null)
{
    //per-user seen state
    //(in real world scenario this should be persisted (e.g. db)
    private readonly Dictionary<Guid, HashSet<Guid>> seenQuotes = new();
    private readonly Random rng = Random.Shared;

    public async Task<Quote?> GetRandomAsync(Guid userId)
    {
        if (!seenQuotes.TryGetValue(userId, out var seenQuoteIds))
        {
            seenQuoteIds = [];
            seenQuotes[userId] = seenQuoteIds;
        }

        var allQuotes = await quoteRepository.GetAllAsync();

        if (allQuotes.Count == 0 || seenQuoteIds.Count == allQuotes.Count)
            return null;

        var unseenQuotes = allQuotes
            .Where(q => !seenQuoteIds.Contains(q.Id))
            .ToList();

        if (unseenQuotes.Count == 0)
            return null;

        var randomQuoteIndex = rng.Next(unseenQuotes.Count);
        var quote = unseenQuotes[randomQuoteIndex];

        // Mark as seen so it won't be picked again for this user
        seenQuoteIds.Add(quote.Id);

        return quote;
    }

    public void Reset(Guid userId)
    {
        seenQuotes.Remove(userId);
    }

    public async Task<bool> TryAddToFavoritesAsync(Guid userId, Guid quoteId)
    {
        // TODO: check if already exists
        
        return await favoritesRepository.TryAddAsync(userId, quoteId);
    }

    public async Task ClearFavoritesAsync(Guid userId)
    {
        // TODO
        return;
    }

    public async Task<IReadOnlyList<Favorite>> GetFavoritesAsync(Guid userId)
    {
        return await favoritesRepository.GetAllAsync();
    }
}