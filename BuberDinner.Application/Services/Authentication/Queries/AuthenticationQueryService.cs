using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Persistence;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication.Queries;

public class AuthenticationQueryService : IAuthenticationQueryService
{
    private readonly IJwtTokenGenerator _jwtGen;

    private readonly IUserRepository _userRepository;

    public AuthenticationQueryService(IJwtTokenGenerator jwtGen, IUserRepository userRepository)
    {
        _jwtGen = jwtGen;
        _userRepository = userRepository;
    }

    async public Task<ErrorOr<AuthenticationResult>> Login(string Email, string Password)
    {
        if (await _userRepository.GetUserAsync(Email, Password) is not User user)
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