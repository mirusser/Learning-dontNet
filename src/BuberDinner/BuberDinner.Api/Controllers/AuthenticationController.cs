using BuberDinner.Application.Authentication.Commands.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Authentication.Register;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthenticationController : ApiController
{
    private readonly ISender mediator;
    private readonly IMapper mapper;

    public AuthenticationController(
        ISender mediator,
        IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = mapper.Map<RegisterCommand>(request);

        ErrorOr<AuthenticationResult> registerResult = await mediator.Send(command);

        return registerResult.Match(
            authResult => Ok(mapper.Map<AuthenticationResponse>(authResult)),
            errors => base.Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = mapper.Map<LoginQuery>(request);

        ErrorOr<AuthenticationResult> loginResult = await mediator.Send(query);

        if (loginResult.IsError && loginResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: loginResult.FirstError.Description);
        }

        return loginResult.Match(
            authResult => Ok(mapper.Map<AuthenticationResponse>(authResult)),
            errors => base.Problem(errors));
    }
}