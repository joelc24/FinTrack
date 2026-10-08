using FinTrack.Domain.Common;

namespace FinTrack.Domain.Incomes;

public sealed class Income : BaseEntity
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public Money Amount { get; private set; } = Money.Zero;
    public DateOnly IncomeDate { get; private set; }
    public Guid ProfileId { get; private set; }

    private Income() { }

    public static Result<Income> Create(string description, Money amount, DateOnly incomeDate, Guid profileId, DateOnly today)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Income>(IncomeErrors.EmptyDescription);

        if (profileId == Guid.Empty)
            return Result.Failure<Income>(IncomeErrors.InvalidProfile);

        var dateValidation = ValidateIncomeDate(incomeDate, today);
        if (dateValidation.IsFailure)
            return Result.Failure<Income>(dateValidation.Error);

        return Result.Success(new Income
        {
            Id = Guid.CreateVersion7(),
            Description = description.Trim(),
            Amount = amount,
            IncomeDate = incomeDate,
            ProfileId = profileId
        });
    }

    public Result UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure(IncomeErrors.EmptyDescription);

        Description = description.Trim();
        return Result.Success();
    }

    public void UpdateAmount(Money amount) => Amount = amount;

    public Result UpdateIncomeDate(DateOnly incomeDate, DateOnly today)
    {
        var validation = ValidateIncomeDate(incomeDate, today);
        if (validation.IsFailure)
            return validation;

        IncomeDate = incomeDate;
        return Result.Success();
    }
    private static Result ValidateIncomeDate(DateOnly incomeDate, DateOnly today)
    {
        var sixMonthsAgo = today.AddMonths(-6);
        if (incomeDate > today || incomeDate < sixMonthsAgo)
            return Result.Failure(IncomeErrors.InvalidRegistrationDate(sixMonthsAgo, today));
        return Result.Success();
    }
}