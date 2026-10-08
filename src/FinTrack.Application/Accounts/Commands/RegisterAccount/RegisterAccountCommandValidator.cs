using FluentValidation;

namespace FinTrack.Application.Accounts.Commands.RegisterAccount;

public sealed class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress()
            .WithMessage("El correo electrónico no tiene un formato válido.");

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage("La contraseña es obligatoria.");

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