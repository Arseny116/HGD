using FluentValidation;
using System.Net;
using System.Text.Json;

namespace MiroIntegration.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (ValidationException exception) { await Write(context, HttpStatusCode.UnprocessableEntity, exception.Errors.Select(x => x.ErrorMessage)); }
        catch (UnauthorizedAccessException exception) { await Write(context, HttpStatusCode.Unauthorized, [exception.Message]); }
        catch (KeyNotFoundException exception) { await Write(context, HttpStatusCode.NotFound, [exception.Message]); }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled API exception");
            await Write(context, HttpStatusCode.InternalServerError, ["An unexpected error occurred."]);
        }
    }

    private static async Task Write(HttpContext context, HttpStatusCode status, IEnumerable<string> errors)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { errors }));
    }
}