using FinTrack.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebApi.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        // 1. Caso de uso: Errores estructurales de FluentValidation (400 Bad Request)
        if (exception is ValidationApplicationException validationException)
        {
            var validationProblem = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de validación",
                Detail = "Uno o más campos del request no cumplen con el formato requerido.",
                Type = "https://fintrack.local",
                Instance = httpContext.Request.Path,
                Extensions = 
                { 
                    ["traceId"] = httpContext.TraceIdentifier,
                    ["errors"] = validationException.Errors // Diccionario campo -> lista de errores
                }
            };

            httpContext.Response.StatusCode = validationProblem.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(validationProblem, cancellationToken);
            return true;
        }

        // 2. Caso por defecto: Excepciones no controladas (500 Internal Server Error)
        _logger.LogError(exception, "Excepción no controlada {Message}", exception.Message);

        var internalProblem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Ha ocurrido un error inesperado",
            Detail = "El equipo ya fue notificado, intenta de nuevo más tarde.",
            Type = "https://api.fintrack.local/errors/server-error",
            Instance = httpContext.Request.Path,
            Extensions = { ["traceId"] = httpContext.TraceIdentifier }
        };

        httpContext.Response.StatusCode = internalProblem.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(internalProblem, cancellationToken);
        return true;
    }
}
