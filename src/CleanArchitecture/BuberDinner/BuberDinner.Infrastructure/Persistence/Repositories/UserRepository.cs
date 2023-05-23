using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.UserAggregate;

namespace BuberDinner.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private static readonly List<User> users = new();

    public void Add(User user)
    {
        if (user.Id is null)
        {
            user = User.Create(
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password);
        }

        users.Add(user);
    }

    public User? GetUserByEmail(string email)
    {
        return users.SingleOrDefault(x => x.Email == email);
    }
}