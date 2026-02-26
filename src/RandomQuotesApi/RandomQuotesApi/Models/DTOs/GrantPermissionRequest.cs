namespace RandomQuotesApi.Models.DTOs;

public sealed record GrantPermissionRequest(Guid UserId, string PermissionName);