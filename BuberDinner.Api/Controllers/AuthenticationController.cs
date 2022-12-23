using BuberDinner.Application.Services.Authentication;
using BuberDinner.Application.Services.Authentication.Commands;
using BuberDinner.Application.Services.Authentication.Queries;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{

    private readonly IAuthenticationCommandService _authCommandService;

    private readonly IAuthenticationQueryService _authQueryService;


    public AuthenticationController(
        IAuthenticationCommandService authCommandService,
        IAuthenticationQueryService authQueryService)
    {
        _authCommandService = authCommandService;
        _authQueryService = authQueryService;
    }

    [HttpPost("register")]
    async public Task<IActionResult> Register(RegisterRequest request)
    {
        var registerResult = await _authCommandService.Register(request.FirstName, request.LastName, request.Email, request.Password);

        return registerResult.Match(
            authResult => Ok(authResult),
            errors => Problem(errors)
        );
    }

    [HttpPost("login")]
    async public Task<IActionResult> Login(LoginRequest request)
    {
        var loginResult = await _authQueryService.Login(request.Email, request.Password);

        if (loginResult.IsError && loginResult.FirstError == Errors.User.InvalidCredentials)
        {
            return Problem(statusCode: 401, detail: loginResult.FirstError.Description);
        }

        return loginResult.Match(
            authResult => Ok(authResult),
            errors => Problem(errors)
        );
    }

}