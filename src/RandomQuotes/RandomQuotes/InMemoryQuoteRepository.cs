using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RandomQuotes.Models;

namespace RandomQuotes;

public interface IQuoteRepository
{
    Task<IReadOnlyList<Quote>> GetAllAsync(CancellationToken ct = default);
    Task<Quote?> GetAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Quote quote, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}

public class InMemoryQuoteRepository
{
    private readonly List<Quote> allQuotes = [];

    public InMemoryQuoteRepository()
    {
        GetAll();
    }

    public IReadOnlyList<Quote> GetAll()
    {
        if (allQuotes.Count == 0)
        {
            LoadQuotes();
        }
        
        return allQuotes;
    }
    
    private void LoadQuotes()
    {
        const string  fileName = "Quotes.json";
        var path = Path.Combine(AppContext.BaseDirectory, fileName);

        if (!File.Exists(path))
            throw new FileNotFoundException($"Could not find {fileName} at {path}.");

        var json = File.ReadAllText(path);

        var deserialized = JsonSerializer.Deserialize<List<Quote>>(json);
        if (deserialized is null)
            throw new InvalidOperationException($"{fileName} deserialized to null.");

        allQuotes.Clear();
        allQuotes.AddRange(deserialized);
    }

    public Quote Get(Guid id)
    {
        return allQuotes.FirstOrDefault(x => x.Id == id);
    }
}

public class QuoteRepository : IQuoteRepository
{
    private readonly AppDbContext db;

    public QuoteRepository(AppDbContext db)
    {
        this.db = db;
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
}
