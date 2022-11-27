using ErrorOr;

namespace BuberDinner.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "Email is already in use");

        public static Error EmailDoesntExist => Error.Conflict(
            code: "User.EmailDoesntExist",
            description: "User with given email does not exists");

        public static Error InvalidPassword => Error.Conflict(
            code: "User.InvalidPassword",
            description: "Invalid password");
    }
}