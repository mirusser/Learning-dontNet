using FluentValidation;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginQueryValidator :
    AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(l => l.Email).NotEmpty();
        RuleFor(l => l.Password).NotEmpty();
    }
}