using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberDinner.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        HttpContext.Items["errors"] = errors;

        return Problem(errors.First());
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => 409,
            ErrorType.Validation => 400,
            _ => 500
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelState.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(
            statusCode: 400,
            title: "There are one or more validation errors",
            modelStateDictionary: modelState);
    }
}