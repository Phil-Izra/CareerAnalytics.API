using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.CareerEvents.ValueObjects;

public sealed class DateRange : ValueObject
{
    public DateTime Start { get; }
    public DateTime? End { get; }

    private DateRange(DateTime start, DateTime? end)
    {
        Start = start;
        End = end;
    }

    public static DateRange Create(DateTime start, DateTime? end = null)
    {
        if (end.HasValue && end.Value < start)
            throw DomainException.Create("DateRange.EndBeforeStart", "End date cannot be before start date.");

        if (start > DateTime.UtcNow.AddDays(1))
            throw DomainException.Create("DateRange.StartInFuture", "Event start date cannot be in the future.");

        return new DateRange(start, end);
    }

    public bool IsOngoing => End is null;

    public int? DurationInMonths =>
        End.HasValue
            ? (int)((End.Value - Start).TotalDays / 30.44)
            : null;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End ?? DateTime.MinValue;
    }
}
