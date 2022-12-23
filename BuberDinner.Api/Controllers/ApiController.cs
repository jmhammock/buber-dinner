using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {

        HttpContext.Items["errors"] = errors;

        var firstError = errors.First();

        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => 409,
            ErrorType.Validation => 400,
            _ => 500
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}