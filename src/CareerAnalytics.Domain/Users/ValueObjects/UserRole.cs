using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Users.ValueObjects;

public sealed class UserRole : ValueObject
{
    public static readonly UserRole User = new("User");
    public static readonly UserRole Admin = new("Admin");

    public string Value { get; }

    private UserRole(string value) => Value = value;

    public static UserRole From(string value) =>
        value switch
        {
            "User" => User,
            "Admin" => Admin,
            _ => throw DomainException.Create("UserRole.Invalid", $"'{value}' is not a valid role.")
        };

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
