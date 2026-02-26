using Microsoft.EntityFrameworkCore;
using RandomQuotes;
using RandomQuotes.Models;

InMemoryQuoteRepository inMemoryQuoteRepository = new();
var inMemoryQuotes = inMemoryQuoteRepository.GetAll();

await using var db = new AppDbContext();
await db.Database.MigrateAsync();

var usersCount = db.Users.Count();

UserRepository userRepository = new UserRepository(db);
var user = await userRepository.GetFirstOrCreateAsync();

QuoteRepository quoteRepository = new QuoteRepository(db);
FavoritesRepository favoritesRepository = new FavoritesRepository(db);

var quotes = await quoteRepository.GetAllAsync();

if (quotes.Count == 0)
{
    foreach (var inMemoryQuote in inMemoryQuotes)
    {
        await quoteRepository.AddAsync(inMemoryQuote);
    }
}

QuoteService quoteService = new(quoteRepository, favoritesRepository);

var currentUserId = user.Id;

while (true)
{
    Console.Clear();

    Console.WriteLine("=== Random Quote App ===");
    Console.WriteLine("1) Show random quote");
    Console.WriteLine("2) Reset seen quotes for current user");
    Console.WriteLine("3) Show all favorites quotes for current user");
    Console.WriteLine("4) Clear favorites quotes for current user");
    Console.WriteLine("0) Exit");
    Console.Write("Choose: ");

    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            await ShowRandomQuoteAsync(currentUserId);
            break;

        case "2":
            quoteService.Reset(currentUserId);
            Console.WriteLine("Seen quotes were reset for the current user.");
            Pause();
            break;

        case "3":
            var favorites = await quoteService.GetFavoritesAsync(currentUserId);
            Console.WriteLine("Favorite quotes:");

            foreach (var favorite in favorites)
            {
                var quote = await quoteRepository.GetAsync(favorite.QuoteId);
                var favoredTimes = await favoritesRepository.HowManyUsersAddedAsFavoriteAsync(quote.Id);

                ShowQuote(quote, favoredTimes);
            }

            Pause();
            break;

        case "4":
            await favoritesRepository.ClearAsync(currentUserId);
            Console.WriteLine("Clear favorites quotes for current user.");
            Pause();
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Unknown option.");
            Pause();
            break;
    }
}

async Task ShowRandomQuoteAsync(Guid userId)
{
    var quote = await quoteService.GetRandomAsync(userId);

    if (quote is null)
    {
        Console.WriteLine("No more unseen quotes for this user (or no quotes available).");
        Pause();
    }
    else
    {
        var favoredTimes = await favoritesRepository.HowManyUsersAddedAsFavoriteAsync(quote.Id);
        ShowQuote(quote, favoredTimes);

        await FavoritesManagementAsync(userId, quote);
    }
}

async Task FavoritesManagementAsync(Guid userId, Quote quote)
{
    Console.WriteLine("F add favorites,R remove from favorites, any other key to continue...");
    var input = Console.ReadKey(intercept: true);

    switch (input.Key)
    {
        case ConsoleKey.F:
            await AddToFavoritesAsync(userId, quote.Id);
            break;

        case ConsoleKey.R:
            await favoritesRepository.TryRemoveAsync(userId, quote.Id);
            break;
    }
}

void ShowQuote(Quote quote, int? favoredTimes = null)
{
    Console.WriteLine();
    Console.WriteLine($"\"{quote.Text}\"");
    Console.WriteLine($"~ {quote.Author}");
    Console.WriteLine($"Origin: {quote.Origin}");
    if (quote.Categories?.Length > 0)
    {
        Console.WriteLine($"Categories: {string.Join(", ", quote.Categories)}");
    }

    favoredTimes = favoredTimes > 0 ? favoredTimes : 0;
    Console.WriteLine($"Favored times: {favoredTimes}");
}

void Pause()
{
    Console.WriteLine();
    Console.Write("Press Enter to continue...");
    Console.ReadLine();
}

async Task AddToFavoritesAsync(Guid userId, Guid quoteId)
{
    Console.WriteLine(await quoteService.TryAddToFavoritesAsync(userId, quoteId)
        ? "Added to favorites"
        : "Already added to favorites");
}