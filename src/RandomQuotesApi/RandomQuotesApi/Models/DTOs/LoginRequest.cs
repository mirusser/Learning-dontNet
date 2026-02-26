namespace RandomQuotesApi.Models.DTOs;

public sealed record LoginRequest(
    string Username,
    string Password);