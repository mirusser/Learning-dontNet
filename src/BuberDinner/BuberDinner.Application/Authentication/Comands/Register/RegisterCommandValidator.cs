using BuberDinner.Application.Authentication.Register;
using FluentValidation;

namespace BuberDinner.Application.Authentication.Comands.Register;

public class RegisterCommandValidator :
    AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(r => r.FirstName).NotEmpty();
        RuleFor(r => r.LastName).NotEmpty();
        RuleFor(r => r.Email).NotEmpty();
        RuleFor(r => r.Password).NotEmpty();
    }
}