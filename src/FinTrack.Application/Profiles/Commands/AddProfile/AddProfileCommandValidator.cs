using FluentValidation;

namespace FinTrack.Application.Profiles.Commands.AddProfile;

public sealed class AddProfileCommandValidator : AbstractValidator<AddProfileCommand>
{
    public AddProfileCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.");

        RuleFor(command => command.LastName)
            .NotEmpty()
            .WithMessage("El apellido es obligatorio.");

        RuleFor(command => command.UserName)
            .NotEmpty()
            .WithMessage("El nombre de usuario es obligatorio.");
    }
}