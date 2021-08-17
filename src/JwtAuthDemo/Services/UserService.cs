using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthDemo.Models;

namespace JwtAuthDemo.Services
{
    public class UserService : IUserService
    {
        //TODO: change using hardcoded values 
        //Uses example data
        public bool IsValidUserInformation(LoginModel model)
        {
            if (model.UserName.Equals("John") && model.Password.Equals("123456")) return true;
            else return false;
        }

        //Uses example data, real use would be to get user from database or api
        public LoginModel GetUserDetails()
        {
            var login = new LoginModel()
            {
                UserName = "John",
                Password = "123456"
            };
            return login;
        }
    }
}
