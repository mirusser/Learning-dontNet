namespace RandomQuotesApi.Models;

public class User
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;

    public ICollection<Favorite> Favorites { get; init; } = [];
    public ICollection<Seen> SeenQuotes { get; init; } = [];
    public ICollection<UserPermission> Permissions { get; init; } = [];
}