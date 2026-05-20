using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketFlow.Api.Exceptions;

namespace TicketFlow.Api.Middleware;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred while processing the request. TraceId: {traceId}",
            httpContext.TraceIdentifier);

        var (statusCode, title) = MapException(exception);

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = GetProblemType(statusCode),
            Instance = httpContext.Request.Path,
            Detail = GetSafeErrorMessage(exception, httpContext),
            Extensions =
            {
                ["traceId"] = httpContext.TraceIdentifier,
                ["timestamp"] = DateTime.UtcNow
            }
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }

    private static (int StatusCode, string Title) MapException(Exception ex) => ex switch
    {
        AppException appEx => ((int)appEx.StatusCode, appEx.Message),
        ArgumentNullException or ArgumentException => (StatusCodes.Status400BadRequest, "Invalid argument."),
        UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized."),
        _ => (StatusCodes.Status500InternalServerError, "Unexpected error.")
    };

    private static string GetProblemType(int statusCode) => statusCode switch
    {
        400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
        401 => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
        403 => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
        404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
        409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
        _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1"
    };

    private static string? GetSafeErrorMessage(Exception exception, HttpContext context)
    {
        var env = context.RequestServices.GetRequiredService<IHostEnvironment>();
        if (env.IsDevelopment())
        {
            return exception.Message;
        }

        return exception is AppException ? exception.Message : null;
    }
}