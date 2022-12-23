using BuberDinner.Application.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginCommandHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtGen;

    private readonly IUserRepository _userRepository;

    public LoginCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtGen)
    {
        _userRepository = userRepository;
        _jwtGen = jwtGen;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetUserAsync(query.Email, query.Password) is not User user)
        {
            return Errors.User.InvalidCredentials;
        }

        var token = _jwtGen.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token
        );
    }
}