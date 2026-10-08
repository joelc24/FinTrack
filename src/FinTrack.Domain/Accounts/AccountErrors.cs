using FinTrack.Domain.Common;

namespace FinTrack.Domain.Accounts;

public static class AccountErrors
{
    public static Error EmptyEmail => Error.Validation("Account.EmptyEmail", "El email es obligatorio.");
    public static Error InvalidEmail => Error.Validation("Account.InvalidEmail", "El formato del email no es válido.");
    public static Error EmptyPassword => Error.Validation("User.EmptyPassword", "La contraseña es obligatoria.");
    public static Error EmailAlreadyExists => Error.Validation("User.EmailAlreadyExists", "El email proporcionado ya ha sido tomado.");
    public static Error InvalidCredentials => Error.Validation("User.InvalidCredentials", "El correo o la contraseña es invalidas.");
}