using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Profiles.ValueObjects;

public sealed class ProfileTheme : ValueObject
{
    public static readonly ProfileTheme Default = new("default");
    public static readonly ProfileTheme Dark = new("dark");
    public static readonly ProfileTheme Minimal = new("minimal");
    public static readonly ProfileTheme Executive = new("executive");

    private static readonly HashSet<string> ValidThemes = ["default", "dark", "minimal", "executive"];

    public string Value { get; }

    private ProfileTheme(string value) => Value = value;

    public static ProfileTheme From(string value)
    {
        var normalized = value.Trim().ToLowerInvariant();
        if (!ValidThemes.Contains(normalized))
            throw DomainException.Create("ProfileTheme.Invalid", $"'{value}' is not a valid profile theme.");

        return new ProfileTheme(normalized);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
