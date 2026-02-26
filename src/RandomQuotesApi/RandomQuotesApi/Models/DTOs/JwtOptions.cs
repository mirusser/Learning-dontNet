namespace RandomQuotesApi.Models.DTOs;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SigningKey { get; init; } = string.Empty;
    public int ExpMinutes { get; init; } = 60;
}