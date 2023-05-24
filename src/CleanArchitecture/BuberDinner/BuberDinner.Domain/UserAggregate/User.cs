using BuberDinner.Domain.Common.Models;
using BuberDinner.Domain.UserAggregate.ValueObjects;

namespace BuberDinner.Domain.UserAggregate;

public sealed class User : AggregateRoot<UserId, Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    private User() { }

    public User(
        UserId userId,
        string firstName,
        string lastName,
        string email,
        string password) : base(userId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    } 

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string password)
    {
        return new (
            UserId.CreateUnique(),
            firstName,
            lastName,
            email,
            password);
    }
}