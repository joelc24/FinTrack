using FinTrack.Domain.Common;

namespace FinTrack.Domain.Incomes;

public static class IncomeErrors
{
    public static readonly Error EmptyDescription =
        Error.Validation("Income.EmptyDescription", "La descripción es obligatoria.");

    public static readonly Error InvalidProfile =
        Error.Validation("Income.InvalidProfile", "El perfil es obligatorio.");
    public static readonly Error NotOwner =
        Error.Validation("Income.NotOwner", "El perfil no te pertenece.");

    public static Error InvalidRegistrationDate(DateOnly minDate, DateOnly maxDate) =>
        Error.Validation("Income.InvalidRegistrationDate",
            $"La fecha del ingreso debe estar entre {minDate:yyyy-MM-dd} y {maxDate:yyyy-MM-dd}.");

    public static Error NotFound(Guid id) =>
        Error.NotFound("Income.NotFound", $"No se encontró el ingreso con id '{id}'.");
}