using System.Text.Json.Serialization;

namespace RandomQuotes.Models;

// public record Quote(Guid Id, string Text, string Author, string Origin, string[] Categories);

public class Quote
{
    [JsonIgnore]
    public Guid Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string Origin { get; init; } = string.Empty;

    // Stored via a value converter (configured it in AppDbContext)
    public string[] Categories { get; init; } = [];

    // Navigation (optional for later)
    public ICollection<Favorite> Favorites { get; init; } = new List<Favorite>();
}