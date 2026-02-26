using AutoFixture;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RandomQuotesApi.Models;
using RandomQuotesApi.Repos;
using RandomQuotesApi.Services;

namespace Tests;

public class QuoteServiceInMemoryTests : IAsyncLifetime
{
    private SqliteConnection sqLiteConnection = null!;
    private AppDbContext db = null!;

    public async Task InitializeAsync()
    {
        sqLiteConnection = new SqliteConnection("Filename=:memory:");
        await sqLiteConnection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(sqLiteConnection)
            .Options;

        db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await db.DisposeAsync();
        await sqLiteConnection.DisposeAsync();
    }

    [Fact]
    public async Task GetRandomAsync_ReturnsDifferentQuotes_UntilAllSeen()
    {
        //await using var db = CreateSqliteContext();

        // Seed quotes using AutoFixture
        var fixture = CreateFixture();

        var quotes = fixture.Build<Quote>()
            .Without(q => q.Favorites)
            .Without(q => q.SeenByUsers)
            .With(q => q.Id, () => Guid.NewGuid())
            .With(q => q.Categories, () => new[] { "test" })
            .CreateMany(10)
            .ToList();

        db.Quotes.AddRange(quotes);
        await db.SaveChangesAsync();

        var userId = Guid.NewGuid();
        User user = new()
        {
            Id = userId,
            Name = "test"
        };

        db.Users.Add(user);

        await db.SaveChangesAsync();

        var quoteRepo = new QuoteRepository(db);
        var seenRepo = new SeenRepository(db);
        var sut = new QuoteService(quoteRepo, seenRepo);

        var results = new List<Quote?>();

        for (var i = 0; i < 12; i++) // more calls than quotes
        {
            results.Add(await sut.GetRandomAsync(userId));
        }

        var nonNull = results.Where(q => q is not null).ToList();

        // Should not see more unique quotes than exist
        Assert.True(nonNull.Count <= quotes.Count);

        // After enough calls, everything should be seen and service returns null
        Assert.Null(results.Last());

        var distinctIds = nonNull.Select(q => q!.Id).Distinct().Count();
        Assert.Equal(nonNull.Count, distinctIds);
    }

    private static Fixture CreateFixture()
    {
        // Seed quotes using AutoFixture
        var fixture = new Fixture();

        // Remove the default behavior that throws on recursion
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));

        // Add a behavior that just omits (EF navigational) properties when recursion is detected
        // I don't need fixture to populate deep navigational properties
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Customize<Quote>(c => c
            .Without(q => q.Favorites)
            .Without(q => q.SeenByUsers));

        fixture.Customize<User>(c => c
            .Without(u => u.Favorites)
            .Without(u => u.SeenQuotes));

        fixture.Customize<Seen>(c => c
            .Without(s => s.User)
            .Without(s => s.Quote));

        fixture.Customize<Favorite>(c => c
            .Without(f => f.User)
            .Without(f => f.Quote));

        return fixture;
    }

    // problem with in memory is that it doesn't care about database relations
    // so you could add 'Seen' model without existing user, and it wouldn't complain
    // - don't want that behavior
    
    // leaving as a reference for learning

    // EF Core InMemory Provider:
    // ❌ Does NOT enforce foreign keys
    // ❌ Does NOT behave like a real relational DB
    // ❌ Would have let “Seen” rows be saved without a User
    // ✔ Fastest and simplest
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("Quotes_" + Guid.NewGuid())
            .Options;

        return new AppDbContext(options);
    }
}