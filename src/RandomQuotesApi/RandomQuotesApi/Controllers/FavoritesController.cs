using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandomQuotesApi.Models.Auth;
using RandomQuotesApi.Models.DTOs;
using RandomQuotesApi.Repos;
using RandomQuotesApi.Services;

namespace RandomQuotesApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class FavoritesController(IFavoritesService favorites) : BaseApiController
{
    [Authorize(Policy = Permissions.Favorites.View)]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<IEnumerable<FavoriteQuoteDto>>> ReadAllForUserAsync(Guid userId,
        CancellationToken ct = default)
    {
        var userIdFromToken = GetUserId();

        var favored = await favorites.GetAllForUserAsync(userId, ct);

        return Ok(favored);
    }

    [Authorize(Policy = Permissions.Favorites.Modify)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddFavoriteRequest request, CancellationToken ct)
    {
        var added = await favorites.AddAsync(request, ct);

        if (!added)
        {
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = "Quote is already in favorites.",
                Type = "https://httpstatuses.com/409"
            };

            return Conflict(problem);
        }

        var location = Url.Action(nameof(ReadAllForUserAsync), new { request.UserId });
        return Created(location!, new
        {
            request.UserId,
            request.QuoteId
        });
    }

    [Authorize(Policy = Permissions.Favorites.Modify)]
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> RemoveAllForUserAsync(Guid userId, CancellationToken ct)
    {
        await favorites.RemoveAllForUserAsync(userId, ct);

        return NoContent();
    }

    [Authorize(Policy = Permissions.Favorites.Modify)]
    [HttpDelete("{userId:guid}/{quoteId:guid}")]
    public async Task<IActionResult> Remove(Guid userId, Guid quoteId, CancellationToken ct)
    {
        var removed = await favorites.RemoveAsync(userId, quoteId, ct);

        if (removed)
            return NoContent();

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = "Favorite not found.",
            Type = "https://httpstatuses.com/404"
        };

        return NotFound(problem);
    }
}