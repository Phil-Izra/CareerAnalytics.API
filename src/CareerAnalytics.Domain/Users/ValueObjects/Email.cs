using System.Text.RegularExpressions;
using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Users.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw DomainException.Create("Email.Empty", "Email cannot be empty.");

        if (!EmailRegex.IsMatch(value))
            throw DomainException.Create("Email.Invalid", $"'{value}' is not a valid email address.");

        return new Email(value.Trim().ToLowerInvariant());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
