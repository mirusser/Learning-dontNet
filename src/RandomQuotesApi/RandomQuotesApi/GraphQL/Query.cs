using RandomQuotesApi.Models;
using RandomQuotesApi.Repos;

namespace RandomQuotesApi.GraphQL;

public class Query
{
    // Cursor-based paging for all quotes
    [UsePaging] // Cursor paging (first/after/last/before)
    [UseProjection] // Allow selecting fields
    [UseFiltering] // optional: filter by author, etc.
    [UseSorting] // optional: sort by author, text, etc.
    public IQueryable<Quote> GetQuotes([Service] AppDbContext db)
        => db.Quotes.AsQueryable();
}