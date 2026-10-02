using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MiroIntegration.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, errors) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status422UnprocessableEntity,
                "Validation failed.",
                validationException.Errors.Select(error => error.ErrorMessage).ToArray()),
            UnauthorizedAccessException unauthorizedException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized.",
                new[] { unauthorizedException.Message }),
            KeyNotFoundException notFoundException => (
                StatusCodes.Status404NotFound,
                "Resource not found.",
                new[] { notFoundException.Message }),
            _ => (
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.",
                Array.Empty<string>())
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled API exception");

        httpContext.Response.StatusCode = statusCode;
        problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = errors.Length == 0 ? null : string.Join(" ", errors)
            }
        });

        return true;
    }
}
