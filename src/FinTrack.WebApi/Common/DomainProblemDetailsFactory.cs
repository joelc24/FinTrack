

using FinTrack.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebApi.Common;

public static class DomainProblemDetailsFactory
{
    public static ProblemDetails FromError(Error error, HttpContext httpContext)
    {
        var (statusCode, title, typeSuffix) = error.Type switch
        {
            ErrorType.Validation => (StatusCodes.Status400BadRequest, "Error de validación", "validation-error"),
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "Recurso no encontrado", "not-found"),
            ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflicto de estado", "conflict"),
            ErrorType.Unauthorized => (StatusCodes.Status401Unauthorized, "No autorizado", "unauthorized"),
            ErrorType.Forbidden => (StatusCodes.Status403Forbidden, "Acceso denegado", "forbidden"),
            _ => (StatusCodes.Status500InternalServerError, "Error inesperado", "server-error")
        };

        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = error.Message,
            Type = $"https://api.fintrack.local/errors/{typeSuffix}",
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["errorCode"] = error.Code,
                ["traceId"] = httpContext.TraceIdentifier
            }
        };
    }
}
