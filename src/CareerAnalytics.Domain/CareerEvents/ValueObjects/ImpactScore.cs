using CareerAnalytics.Domain.CareerEvents.Enums;
using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.CareerEvents.ValueObjects;

public sealed class ImpactScore : ValueObject
{
    public int Value { get; }

    private static readonly Dictionary<EventType, int> DefaultScores = new()
    {
        { EventType.JobStarted,   15 },
        { EventType.Promotion,    25 },
        { EventType.Certification, 10 },
        { EventType.Project,      20 },
        { EventType.Leadership,   20 },
        { EventType.Award,        15 },
        { EventType.SkillUpgrade,  8 },
        { EventType.Conference,   18 }
    };

    private ImpactScore(int value) => Value = value;

    public static ImpactScore Create(int value)
    {
        if (value < 0 || value > 100)
            throw DomainException.Create("ImpactScore.OutOfRange", "Impact score must be between 0 and 100.");

        return new ImpactScore(value);
    }

    public static ImpactScore Default(EventType eventType) =>
        new(DefaultScores.TryGetValue(eventType, out var score) ? score : 10);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
