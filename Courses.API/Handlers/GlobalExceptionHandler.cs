using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Courses.API.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Error no controlado en la API. TraceId: {TraceId}",
            httpContext.TraceIdentifier);

        var (statusCode, title, detail) = exception switch
        {
            ArgumentException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Solicitud inválida.",
                    exception.Message
                ),

            KeyNotFoundException =>
                (
                    StatusCodes.Status404NotFound,
                    "Recurso no encontrado.",
                    exception.Message
                ),

            InvalidOperationException =>
                (
                    StatusCodes.Status409Conflict,
                    "Operación no permitida.",
                    exception.Message
                ),

            UnauthorizedAccessException =>
                (
                    StatusCodes.Status403Forbidden,
                    "Acceso denegado.",
                    "No tienes permisos suficientes para realizar esta operación."
                ),

            _ =>
                (
                    StatusCodes.Status500InternalServerError,
                    "Error interno del servidor.",
                    "Ocurrió un error inesperado al procesar la solicitud."
                )
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType =
            "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}