namespace FinTrack.Domain.Common;

public sealed record Money
{
    public decimal Amount { get; }

    private Money(decimal amount) => Amount = amount;

    public static Result<Money> Create(decimal amount)
    {
        if (amount < 0)
            return Result.Failure<Money>(Error.Validation("Money.Negative", "El monto no puede ser negativo."));

        return Result.Success(new Money(amount));
    }

    public Money Add(Money other) => new(Amount + other.Amount);

    public static readonly Money Zero = new(0);

    public static Money operator +(Money left, Money right) => left.Add(right);
    public static bool operator >(Money left, Money right) => left.Amount > right.Amount;
    public static bool operator <(Money left, Money right) => left.Amount < right.Amount;
    public static bool operator >=(Money left, Money right) => left.Amount >= right.Amount;
    public static bool operator <=(Money left, Money right) => left.Amount <= right.Amount;
}