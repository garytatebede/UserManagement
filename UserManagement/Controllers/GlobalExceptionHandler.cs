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
        var (code, title) = GetStatusAndTitle(exception);

        var problemDetails = new ProblemDetails
        {
            Status = code,
            Title = title,
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
        return exception switch
        {
            UserNotFoundException unfe => (StatusCodes.Status404NotFound, $"User with id: {unfe.Id} not found"),
            UserExistsException uee => (StatusCodes.Status409Conflict, $"User with username: {uee.Username} already exists"),
            _ => (StatusCodes.Status503ServiceUnavailable, "Service Unavailable")
        };

        // Either are get the same result, but I prefer the switch expression

        //if (exception is UserNotFoundException unfe)
        //{
        //    return (StatusCodes.Status404NotFound, $"User with id: {unfe.Id} not found");
        //}

        //if (exception is UserExistsException uee)
        //{
        //    return (StatusCodes.Status409Conflict, $"User with username: {uee.Username} already exists");
        //}

        // return (StatusCodes.Status503ServiceUnavailable, "Service Unavailable");
    }
}
