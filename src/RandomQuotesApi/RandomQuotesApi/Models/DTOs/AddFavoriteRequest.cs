using System.ComponentModel.DataAnnotations;

namespace RandomQuotesApi.Models.DTOs;

public sealed record AddFavoriteRequest(
    [Required] Guid UserId,
    [Required] Guid QuoteId);