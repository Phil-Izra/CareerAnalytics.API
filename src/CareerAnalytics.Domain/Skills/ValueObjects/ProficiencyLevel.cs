using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Skills.ValueObjects;

/// <summary>
/// 1 = Beginner, 2 = Basic, 3 = Intermediate, 4 = Advanced, 5 = Expert
/// </summary>
public sealed class ProficiencyLevel : ValueObject
{
    public int Value { get; }

    private static readonly Dictionary<int, string> Labels = new()
    {
        { 1, "Beginner" },
        { 2, "Basic" },
        { 3, "Intermediate" },
        { 4, "Advanced" },
        { 5, "Expert" }
    };

    private ProficiencyLevel(int value) => Value = value;

    public static ProficiencyLevel Create(int value)
    {
        if (value < 1 || value > 5)
            throw DomainException.Create("ProficiencyLevel.OutOfRange",
                "Proficiency level must be between 1 (Beginner) and 5 (Expert).");

        return new ProficiencyLevel(value);
    }

    public string Label => Labels[Value];

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Label;
}
