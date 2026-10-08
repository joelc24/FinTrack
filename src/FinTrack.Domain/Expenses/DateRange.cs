using FinTrack.Domain.Common;

namespace FinTrack.Domain.Expenses; 

public sealed record DateRange
{
    public DateTime StartDate { get; }
    public DateTime FinishDate { get; }

    private DateRange(DateTime startDate, DateTime finishDate)
    {
        StartDate = startDate;
        FinishDate = finishDate;
    }

    public static Result<DateRange> Create(DateTime startDate, DateTime finishDate)
    {
        if (startDate > finishDate)
            return Result.Failure<DateRange>(Error.Validation("DateRange.InvalidRange", "La fecha de inicio no puede ser posterior a la fecha límite."));

        return Result.Success(new DateRange(startDate, finishDate));
    }

    
    public bool Contains(DateTime date) => date >= StartDate && date <= FinishDate;

    public bool IsExpired() => DateTime.UtcNow > FinishDate;

    public int DurationInDays => (FinishDate - StartDate).Days;
}