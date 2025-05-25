using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JWTAuthenticationDemo.Constants;
using JWTAuthenticationDemo.Contexts;
using JWTAuthenticationDemo.Models;
using JWTAuthenticationDemo.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthenticationDemo.Services;

public class UserService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<Jwt> jwt,
    ApplicationDbContext context)
    : IUserService
{
    private readonly RoleManager<IdentityRole> roleManager = roleManager;
    private readonly Jwt jwt = jwt.Value;

    public ApplicationUser GetById(string id)
    {
        return context.Users.Find(id);
    }
    
    public async Task<string> RegisterAsync(RegisterModel model)
    {
        ApplicationUser user = new()
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
        var userWithSameEmail = await userManager.FindByEmailAsync(model.Email);
        if (userWithSameEmail is not null)
        {
            return $"Email {user.Email} is already registered.";
        }

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Authorization.DefaultRole.ToString());
        }

        return $"User Registered with username {user.UserName}";
    }

    public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
    {
        AuthenticationModel authenticationModel = new();
        var user = await userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"No Accounts Registered with {model.Email}.";

            return authenticationModel;
        }

        if (await userManager.CheckPasswordAsync(user, model.Password))
        {
            JwtSecurityToken jwtSecurityToken = await CreateJwtTokenAsync(user);

            authenticationModel.IsAuthenticated = true;
            authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.Email = user.Email ?? string.Empty;
            authenticationModel.UserName = user.UserName ?? string.Empty;

            var rolesList = await userManager.GetRolesAsync(user);
            authenticationModel.Roles = rolesList.ToList();

            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(a => a.IsActive);
                if (activeRefreshToken is not null)
                {
                    authenticationModel.RefreshToken = activeRefreshToken.Token;
                    authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
                }
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                authenticationModel.RefreshToken = refreshToken.Token;
                authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                
                context.Update(user);
                await context.SaveChangesAsync();
            }

            return authenticationModel;
        }

        authenticationModel.IsAuthenticated = false;
        authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";

        return authenticationModel;
    }

    public async Task<string> AddToRoleAsync(AddRoleModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            return $"No Accounts Registered with {model.Email}.";
        }

        if (!await userManager.CheckPasswordAsync(user, model.Password))
        {
            return $"Incorrect Credentials for user {user.Email}.";
        }

        var roleExists = Enum.GetNames(typeof(Authorization.Roles)).Any(x => x.ToLower() == model.Role.ToLower());

        if (!roleExists)
        {
            return $"Role {model.Role} not found.";
        }

        var validRole = Enum
            .GetValues<Authorization.Roles>()
            .FirstOrDefault(x => string.Equals(x.ToString(), model.Role, StringComparison.CurrentCultureIgnoreCase));

        await userManager.AddToRoleAsync(user, validRole.ToString());

        return $"Added {model.Role} to user {model.Email}.";
    }

    public async Task<AuthenticationModel> RefreshTokenAsync(string token)
    {
        AuthenticationModel authenticationModel = new();
        var user = context.Users.SingleOrDefault(x => x.RefreshTokens.Any(a => a.Token == token));
        if (user is null)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"No Accounts Registered with {token}.";
            
            return authenticationModel;
        }
        
        var refreshToken = user.RefreshTokens.Single(a => a.Token == token);

        if (!refreshToken.IsActive)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Token is not active";
            
            return authenticationModel;
        }
        
        refreshToken.Revoked = DateTime.UtcNow;
        
        var newRefreshToken = CreateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        context.Update(user);
        await context.SaveChangesAsync();
        
        var jwtSecurityToken = await CreateJwtTokenAsync(user);

        authenticationModel.IsAuthenticated = true;
        authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authenticationModel.Email = user.Email ?? string.Empty;
        authenticationModel.UserName = user.UserName ?? string.Empty;

        var rolesList = await userManager.GetRolesAsync(user);
        authenticationModel.Roles = rolesList.ToList();
        
        authenticationModel.RefreshToken = newRefreshToken.Token;
        authenticationModel.RefreshTokenExpiration = newRefreshToken.Expires;
        
        return authenticationModel;
    }

    public async Task<bool> RevokeToken(string token)
    {
        var user = context.Users.SingleOrDefault(x => x.RefreshTokens.Any(a => a.Token == token));

        if (user is null)
        {
            return false;
        }
        
        var refreshToken = user.RefreshTokens.Single(a => a.Token == token);

        if (!refreshToken.IsActive)
        {
            return false;
        }
        
        refreshToken.Revoked = DateTime.UtcNow;
        context.Update(user);
        await context.SaveChangesAsync();
        
        return true;
    }
    private async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        var roleClaims = roles
            .Select(r => new Claim("roles", r))
            .ToList();

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwt.DurationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }

    private RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        RandomNumberGenerator.Create().GetBytes(randomNumber);

        return new RefreshToken()
        {
            Token = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.AddDays(10),
            Created = DateTime.UtcNow
        };
    }
}