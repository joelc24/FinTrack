using FluentValidation;

namespace FinTrack.Application.Profiles.Commands.UpdateProfile;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(command => command.ProfileId)
            .NotEmpty()
            .WithMessage("El identificador del perfil debe ser un GUID válido.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.");

        RuleFor(command => command.LastName)
            .NotEmpty()
            .WithMessage("El apellido es obligatorio.");
    }
}