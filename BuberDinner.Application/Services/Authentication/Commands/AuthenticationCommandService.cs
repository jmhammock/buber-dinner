using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Persistence;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication.Commands;

public class AuthenticationCommandService : IAuthenticationCommandService
{
    private readonly IJwtTokenGenerator _jwtGen;

    private readonly IUserRepository _userRepository;

    public AuthenticationCommandService(IJwtTokenGenerator jwtGen, IUserRepository userRepository)
    {
        _jwtGen = jwtGen;
        _userRepository = userRepository;
    }

    async public Task<ErrorOr<AuthenticationResult>> Register(string firstName, string lastName, string email, string password)
    {
        //Check if user already exists
        if (await _userRepository.GetUserAsync(email) != null)
        {
            return Errors.User.DuplicateEmail;
        }

        var user = await _userRepository.AddUserAsync(new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        });

        //Create JWT token
        var token = _jwtGen.GenerateToken(user);

        return new AuthenticationResult(
            user,
            token
        );
    }

}