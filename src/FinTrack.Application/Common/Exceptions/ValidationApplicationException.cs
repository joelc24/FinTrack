using FluentValidation.Results;

namespace FinTrack.Application.Common.Exceptions;

public sealed class ValidationApplicationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationApplicationException(IEnumerable<ValidationFailure> failures)
        : base("Uno o más errores de validación han ocurrido.")
    {
        Errors = failures
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
    }
}
