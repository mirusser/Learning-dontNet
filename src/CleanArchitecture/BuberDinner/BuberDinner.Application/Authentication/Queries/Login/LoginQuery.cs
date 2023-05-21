using BuberDinner.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login;

public record class LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;