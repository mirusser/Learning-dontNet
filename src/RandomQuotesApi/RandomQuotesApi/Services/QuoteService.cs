using RandomQuotesApi.Models;
using RandomQuotesApi.Repos;

namespace RandomQuotesApi.Services;

public class QuoteService(
    IQuoteRepository quoteRepository,
    ISeenRepository seenRepository)
{
    public async Task<Quote?> GetRandomAsync(Guid userId, CancellationToken ct = default)
    {
        var quote = await quoteRepository.GetRandomUnseenAsync(userId, ct);
        if (quote is null)
        {
            return null;
        }

        await seenRepository.MarkSeenAsync(userId, quote.Id, ct);
        
        return quote;
    }

    public async Task ClearSeenAsync(Guid userId, CancellationToken ct = default)
    {
        await seenRepository.ClearSeenAsync(userId, ct);
    }
}