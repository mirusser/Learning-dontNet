using JwtAuthDemo.Models;

namespace JwtAuthDemo.Services
{
    public interface IUserService
    {
        bool IsValidUserInformation(LoginModel model);
        LoginModel GetUserDetails();
    }
}