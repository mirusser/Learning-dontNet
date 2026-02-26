using Microsoft.AspNetCore.Mvc;
using RandomQuotesApi.Clients;
using RandomQuotesApi.Models;
using RandomQuotesApi.Models.DTOs;
using RandomQuotesApi.Repos;

namespace RandomQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository users) : ControllerBase
{
    [HttpGet("first-or-create")]
    public async Task<ActionResult<User>> GetFirstOrCreate(CancellationToken ct)
    {
        var user = await users.GetFirstOrCreateAsync(ct);
        return Ok(user);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<List<User>>> GetAllAsync(CancellationToken ct)
    {
        var allUsers = await users.GetAllAsync(ct);

        // TODO: should be DTO instead of anonymous object
        // and getting this should be done in service, as data retrieval touches more than one table
        var usersDto = allUsers
            .Select(u => new
            {
                u.Id,
                u.Name,
                Permissions = u.Permissions.Select(p => new { p.Permission.Id, p.Permission.Name })
            })
            .ToList();
        return Ok(usersDto);
    }


    [HttpPost("grant-permission")]
    public async Task<IActionResult> GrantPermission([FromBody] GrantPermissionRequest request, CancellationToken ct)
    {
        var ok = await users.GrantPermissionAsync(request.UserId, request.PermissionName, ct);
        if (!ok)
        {
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "User not found",
                Detail = $"User {request.UserId} does not exist."
            });
        }

        return NoContent();
    }

    [HttpGet("permissions")]
    public async Task<ActionResult<List<Permission>>> GetAllPermissions(CancellationToken ct)
    {
        return Ok(await users.GetAllPermissions(ct));
    }
}