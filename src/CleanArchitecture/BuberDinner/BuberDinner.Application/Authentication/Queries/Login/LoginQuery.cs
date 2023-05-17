using BuberDinner.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login;

public record class LoginQuery(
    string FirstName,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;