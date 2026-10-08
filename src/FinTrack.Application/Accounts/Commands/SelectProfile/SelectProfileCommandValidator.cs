using FluentValidation;

namespace FinTrack.Application.Accounts.Commands.SelectProfile;

public sealed class SelectProfileCommandValidator : AbstractValidator<SelectProfileCommand>
{
    public SelectProfileCommandValidator()
    {
        RuleFor(command => command.ProfileId)
            .NotEmpty()
            .WithMessage("El identificador del perfil debe ser un GUID válido.");
    }
}