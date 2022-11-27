using BuberDinner.Application.Authentication.Commands.Common;
using BuberDinner.Application.Authentication.Register;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Comands.Register;

public class RegisterCommandHandler :
    IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator jwtTokenGenerator;
    private readonly IUserRepository userRepository;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository)
    {
        this.jwtTokenGenerator = jwtTokenGenerator;
        this.userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask; //to get rid of the warning (for now)

        // 1. Validate the user doesn't exist
        if (userRepository.GetUserByEmail(command.Email) is not null)
        {
            //Used in flow control with exceptions:
            //throw new DuplicateEmailException("User with given email already exists.");

            //Used in flow control without exceptions:

            //FluentResults package:
            //return Result.Fail<AuthenticationResult>(new[] { new DuplicateEmailError() });

            //ErrorOr package:
            return Errors.User.DuplicateEmail;

            //Default flow control:
            //throw new Exception("User with given email already exists.");
        }

        // 2. Create user (generate unique ID) & persist to db
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Password = command.Password,
        };
        userRepository.Add(user);

        // 3. Create JWT token
        var token = jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token);
    }
}