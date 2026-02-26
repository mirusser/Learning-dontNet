namespace RandomQuotesApi.Models;

public class Seen
{
    // Composite key (UserId, QuoteId)
    public Guid UserId { get; init; }
    public Guid QuoteId { get; init; }

    public User? User { get; init; }
    public Quote? Quote { get; init; }
    
    public DateTime Date { get; init; }
}