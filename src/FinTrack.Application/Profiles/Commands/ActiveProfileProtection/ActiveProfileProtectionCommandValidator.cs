using FluentValidation;

namespace FinTrack.Application.Profiles.Commands.ActiveProfileProtection;

public sealed class ActivateProfileProtectionCommandValidator : AbstractValidator<ActivateProfileProtectionCommand>
{
    public ActivateProfileProtectionCommandValidator()
    {
        RuleFor(command => command.ProfileId)
            .NotEmpty()
            .WithMessage("El identificador del perfil debe ser un GUID válido.");

        RuleFor(command => command.Pin)
            .NotEmpty()
            .WithMessage("El PIN es obligatorio.");
    }
}