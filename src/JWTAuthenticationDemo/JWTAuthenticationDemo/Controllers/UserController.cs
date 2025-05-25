using JWTAuthenticationDemo.Models;
using JWTAuthenticationDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthenticationDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterModel model)
    {
        var result = await userService.RegisterAsync(model);
        return Ok(result);
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
    {
        var result = await userService.GetTokenAsync(model);
        SetRefreshTokenInCookie(result.RefreshToken);

        return Ok(result);
    }

    [HttpPost("addtorole")]
    public async Task<IActionResult> AddToRoleAsync(AddRoleModel model)
    {
        var result = await userService.AddToRoleAsync(model);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("tokens/{id}")]
    public IActionResult GetRefreshTokens(string id)
    {
        var user = userService.GetById(id);
        return Ok(user.RefreshTokens);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await userService.RefreshTokenAsync(refreshToken);
        
        if (!string.IsNullOrEmpty(response.RefreshToken))
        {
            SetRefreshTokenInCookie(response.RefreshToken);
        }

        return Ok(response);
    }
    
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
    {
        // accept token from request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Token is required" });

        var response = await userService.RevokeToken(token);

        if (!response)
            return NotFound(new { message = "Token not found" });

        return Ok(new { message = "Token revoked" });
    }

    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}