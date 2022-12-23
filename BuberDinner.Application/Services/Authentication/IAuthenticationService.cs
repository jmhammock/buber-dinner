using ErrorOr;

namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<ErrorOr<AuthenticationResult>> Register(string firstName, string lastName, string email, string password);
    Task<ErrorOr<AuthenticationResult>> Login(string email, string password);

}