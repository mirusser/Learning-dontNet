using JWTAuthenticationDemo.Constants;
using JWTAuthenticationDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace JWTAuthenticationDemo.Contexts;

public class ApplicationDbContextSeed
{
    public static async Task SeedEssentialsAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(nameof(Authorization.Roles.Administrator)));
        await roleManager.CreateAsync(new IdentityRole(nameof(Authorization.Roles.Moderator)));
        await roleManager.CreateAsync(new IdentityRole(nameof(Authorization.Roles.User)));

        //Seed Default User
        ApplicationUser defaultUser = new () 
        {
            FirstName = Authorization.DefaultFirstName,
            LastName = Authorization.DefaultEmail,
            UserName = Authorization.DefaultUsername, 
            Email = Authorization.DefaultEmail, 
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            await userManager.CreateAsync(defaultUser, Authorization.DefaultPassword);
            await userManager.AddToRoleAsync(defaultUser, Authorization.DefaultRole.ToString());
        }
    }
}