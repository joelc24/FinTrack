using FinTrack.Domain.Common;

namespace FinTrack.Domain.Profiles;

public static class ProfileErrors
{
    public static readonly Error EmptyName = Error.Validation("Profile.EmptyName", "EL campo name es requerido");
    public static readonly Error EmptyLastName = Error.Validation("Profile.EmptyLastName", "EL campo last name es requerido");
    public static readonly Error EmptyUserName = Error.Validation("Profile.EmptyUserName", "EL campo user name es requerido");
    public static readonly Error EmptyPassword = Error.Validation("Profile.EmptyPassword", "La contraseña es requerida para proteger la cuenta");
    public static readonly Error ProfileProtected = Error.Validation("Profile.ProfileProtected", "no se puede activar la proteccion en una cuenta ya protegida");
    public static readonly Error ProfileUnprotected = Error.Validation("Profile.ProfileUnprotected", "no se puede desactivar la proteccion en una cuenta desprotegida");
    public static readonly Error PasswordInvalid = Error.Validation("Profile.PasswordInvalid", "La contraseña proporcionada no es valida.");
    public static readonly Error InvalidAccount = Error.Validation("Profile.InvalidAccount", "La cuenta proporcionada no es valida.");
    public static readonly Error InvalidPin = Error.Validation("Profile.InvalidPin", "El PIN ingresado no es correcto.");
    public static readonly Error NotOwnedByAccount = Error.Validation("Profile.NotOwnedByAccount", "El perfil no seleccionado no pertenece a la cuenta.");
    public static readonly Error NotOwner = Error.Validation("Profile.NotOwner", "No es el propietario.");
    public static Error NotFound(Guid profileId) => Error.Validation("Profile.InvalidAccount", $"perfil con id {profileId} no encontrado.");
}