using JWTAuthenticationDemo.Models;

namespace JWTAuthenticationDemo.Services;

public interface IUserService
{
    Task<string> RegisterAsync(RegisterModel model);
    Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
    Task<string> AddToRoleAsync(AddRoleModel model);
    Task<AuthenticationModel> RefreshTokenAsync(string token);
    ApplicationUser GetById(string id);
    Task<bool> RevokeToken(string token);
}