using BuberDinner.Application.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{

    private readonly IJwtTokenGenerator _jwtGen;

    private readonly IUserRepository _userRepository;
    public RegisterCommandHandler(IJwtTokenGenerator jwtGen, IUserRepository userRepository)
    {
        _jwtGen = jwtGen;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        //Check if user already exists
        if (await _userRepository.GetUserAsync(command.Email) != null)
        {
            return Errors.User.DuplicateEmail;
        }

        var user = await _userRepository.AddUserAsync(new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Password = command.Password
        });

        //Create JWT token
        var token = _jwtGen.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token
        );
    }
}