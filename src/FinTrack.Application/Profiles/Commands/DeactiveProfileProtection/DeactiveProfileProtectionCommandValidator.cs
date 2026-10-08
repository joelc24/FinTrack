using FluentValidation;

namespace FinTrack.Application.Profiles.Commands.DeactiveProfileProtection;

public sealed class DeactivateProfileProtectionCommandValidator : AbstractValidator<DeactivateProfileProtectionCommand>
{
    public DeactivateProfileProtectionCommandValidator()
    {
        RuleFor(command => command.ProfileId)
            .NotEmpty()
            .WithMessage("El identificador del perfil debe ser un GUID válido.");
    }
}