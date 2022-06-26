using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace AuthJWTDemo;

public class JWTAuthenticationManager
{
    private readonly string _key;
    private readonly IDictionary<string, string> users = new Dictionary<string, string>()
    { {"test", "password"}, {"test1", "pwd"}};

    public JWTAuthenticationManager(string key)
    {
        _key = key;
    }

    public string Authenticate(string username, string password)
    {
        if (!users.Any(u => u.Key == username && u.Value == password))
        {
            return null;
        }

        JwtSecurityTokenHandler tokenHandler = new();
        var tokenKey = Encoding.ASCII.GetBytes(_key);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new(
                new SymmetricSecurityKey(tokenKey), 
                SecurityAlgorithms.HmacSha256Signature),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
