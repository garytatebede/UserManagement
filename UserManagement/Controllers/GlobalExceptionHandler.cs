using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Services.Users;

namespace UserManagement.Controllers;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var result = GetStatusAndTitle(exception);

        var problemDetails = new ProblemDetails
        {
            Status = result.Code,
            Title = result.Title,
            Extensions = new Dictionary<string, object?>
            {
                { "errors", new object[]{ exception } }
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int Code, string Title) GetStatusAndTitle(Exception exception)
    {
        if (exception is UserNotFoundException)
        {
            return (StatusCodes.Status404NotFound, "User not found");
        }

        if (exception is UserExistsException)
        {
            return (StatusCodes.Status409Conflict, "User already exists");
        }

        return (StatusCodes.Status503ServiceUnavailable, "Service Unavailable");
    }
}
