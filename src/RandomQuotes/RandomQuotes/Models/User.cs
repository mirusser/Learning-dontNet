namespace RandomQuotes.Models;

public class User
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public ICollection<Favorite> Favorites { get; init; } = new List<Favorite>();
}