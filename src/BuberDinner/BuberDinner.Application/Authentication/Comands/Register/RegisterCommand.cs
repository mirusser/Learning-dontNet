using BuberDinner.Application.Authentication.Commands.Common;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;