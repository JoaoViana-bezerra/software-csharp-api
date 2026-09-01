using System.Net;
using System.Text.Json;

namespace TaskFlow.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionHandlingMiddleware>
        _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(
                context,
                exception
            );
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode =
            exception switch
            {
                ArgumentException =>
                    HttpStatusCode.BadRequest,

                InvalidOperationException =>
                    HttpStatusCode.BadRequest,

                KeyNotFoundException =>
                    HttpStatusCode.NotFound,

                UnauthorizedAccessException =>
                    HttpStatusCode.Unauthorized,

                _ =>
                    HttpStatusCode.InternalServerError
            };

        if (statusCode ==
            HttpStatusCode.InternalServerError)
        {
            _logger.LogError(
                exception,
                "Unhandled exception."
            );
        }
        else
        {
            _logger.LogWarning(
                exception,
                "Handled application exception."
            );
        }

        context.Response.StatusCode =
            (int)statusCode;

        context.Response.ContentType =
            "application/json";

        var response = new
        {
            status = (int)statusCode,

            error = statusCode.ToString(),

            message =
                statusCode ==
                HttpStatusCode.InternalServerError

                ? "An unexpected error occurred."

                : exception.Message
        };

        var json = JsonSerializer.Serialize(
            response,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy =
                    JsonNamingPolicy.CamelCase
            }
        );

        await context.Response.WriteAsync(json);
    }
}