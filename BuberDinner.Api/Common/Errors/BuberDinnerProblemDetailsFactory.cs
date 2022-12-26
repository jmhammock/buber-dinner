using System.Diagnostics;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace BuberDinner.Api.Common.Errors;

public class BuberDinnerProblemDetailsFactory : ProblemDetailsFactory
{

    private readonly ApiBehaviorOptions _options;

    public BuberDinnerProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = title,
            Detail = detail,
            Instance = instance,
            Status = statusCode
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode);

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Type = type,
            Title = title,
            Detail = detail,
            Instance = instance,
            Status = statusCode
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode);

        return problemDetails;

    }

    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int? statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode ?? 0, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId is not null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        var errors = httpContext?.Items["errors"] as List<Error>;

        if (errors is not null)
        {
            problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
        }
    }
}