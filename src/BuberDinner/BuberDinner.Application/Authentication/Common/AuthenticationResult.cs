using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Authentication.Commands.Common;

public record AuthenticationResult(
    User User,
    string Token);