

using FinTrack.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.WebApi.Common;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return NoContent();

        return Problem(DomainProblemDetailsFactory.FromError(result.Error, HttpContext));
    }

    protected IActionResult HandleResult<T>(Result<T> result, Func<T, IActionResult>? onSuccess = null)
    {
        if (result.IsSuccess)
            return onSuccess is not null ? onSuccess(result.Value) : Ok(result.Value);

        return Problem(DomainProblemDetailsFactory.FromError(result.Error, HttpContext));
    }

    private IActionResult Problem(ProblemDetails problemDetails) =>
        new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
}
