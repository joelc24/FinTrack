using FluentValidation;

namespace FinTrack.Application.Accounts.Commands.LoginAccount;

public sealed class LoginAccountCommandValidator : AbstractValidator<LoginAccountCommand>
{
    public LoginAccountCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress()
            .WithMessage("El correo electrónico no tiene un formato válido.");

        RuleFor(command => command.Password)
            .NotEmpty()
            .WithMessage("La contraseña es obligatoria.");
    }
}