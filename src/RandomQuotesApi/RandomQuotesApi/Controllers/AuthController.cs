using Microsoft.AspNetCore.Mvc;
using RandomQuotesApi.Models.DTOs;
using RandomQuotesApi.Repos;
using RandomQuotesApi.Services;

namespace RandomQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserRepository users, IJwtService jwt) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        var user = await users.GetByCredentialsAsync(request.Username, request.Password, ct);

        if (user is null)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid credentials",
                Status = 401
            });
        }

        var permissions = await users.GetPermissionNamesAsync(user.Id, ct);

        var token = jwt.CreateJwt(user.Id, user.Name, permissions);

        return Ok(new { token });
    }
}