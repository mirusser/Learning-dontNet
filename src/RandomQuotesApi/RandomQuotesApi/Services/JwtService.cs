using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RandomQuotesApi.Models.DTOs;

namespace RandomQuotesApi.Services;

public interface IJwtService
{
    string CreateJwt(Guid userId, string username, IEnumerable<string> permissions);
}

public class JwtService(IOptions<JwtOptions> options) : IJwtService
{
    public string CreateJwt(Guid userId, string username, IEnumerable<string> permissions)
    {
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(options.Value.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("username", username)
        };

        // Add dynamic permission claims
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        var token = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}