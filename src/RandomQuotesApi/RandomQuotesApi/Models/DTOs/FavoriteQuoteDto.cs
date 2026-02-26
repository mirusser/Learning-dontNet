namespace RandomQuotesApi.Models.DTOs;

public sealed record FavoriteQuoteDto(
    Guid Id,
    string Text,
    string Author,
    Guid UserId);
