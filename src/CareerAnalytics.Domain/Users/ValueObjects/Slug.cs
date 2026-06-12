using System.Text.RegularExpressions;
using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Users.ValueObjects;

/// <summary>
/// URL-safe identifier used for public profile routes: /u/{slug}
/// </summary>
public sealed class Slug : ValueObject
{
    public string Value { get; }

    private static readonly Regex SlugRegex = new(
        @"^[a-z0-9]+(?:-[a-z0-9]+)*$",
        RegexOptions.Compiled);

    private Slug(string value) => Value = value;

    public static Slug Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw DomainException.Create("Slug.Empty", "Slug cannot be empty.");

        var normalized = value.Trim().ToLowerInvariant();

        if (normalized.Length < 3 || normalized.Length > 60)
            throw DomainException.Create("Slug.InvalidLength", "Slug must be between 3 and 60 characters.");

        if (!SlugRegex.IsMatch(normalized))
            throw DomainException.Create("Slug.InvalidFormat",
                "Slug may only contain lowercase letters, numbers, and hyphens, and cannot start or end with a hyphen.");

        return new Slug(normalized);
    }

    public static Slug FromFullName(string fullName)
    {
        var raw = fullName.Trim().ToLowerInvariant();
        var sanitized = Regex.Replace(raw, @"[^a-z0-9\s-]", "");
        var hyphenated = Regex.Replace(sanitized, @"\s+", "-");
        var trimmed = hyphenated.Trim('-');

        if (trimmed.Length < 3) trimmed = trimmed.PadRight(3, '0');
        if (trimmed.Length > 60) trimmed = trimmed[..60].TrimEnd('-');

        return new Slug(trimmed);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
