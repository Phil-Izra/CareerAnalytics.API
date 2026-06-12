using CareerAnalytics.Domain.Common;
using CareerAnalytics.Domain.Users.Events;
using CareerAnalytics.Domain.Users.ValueObjects;

namespace CareerAnalytics.Domain.Users;

public sealed class User : AggregateRoot
{
    public string FullName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public Slug Slug { get; private set; } = null!;
    public UserRole Role { get; private set; } = null!;
    public bool IsEmailVerified { get; private set; }
    public bool IsPublicProfile { get; private set; }

    private User() { }

    public static User Create(string fullName, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw DomainException.Create("User.FullNameEmpty", "Full name cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw DomainException.Create("User.PasswordHashEmpty", "Password hash cannot be empty.");

        var user = new User
        {
            FullName = fullName.Trim(),
            Email = Email.Create(email),
            PasswordHash = passwordHash,
            Slug = Slug.FromFullName(fullName),
            Role = UserRole.User,
            IsEmailVerified = false,
            IsPublicProfile = true
        };

        user.RaiseDomainEvent(new UserCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            user.Id,
            user.Email.Value,
            user.Slug.Value));

        return user;
    }

    public void VerifyEmail()
    {
        if (IsEmailVerified)
            throw DomainException.Create("User.AlreadyVerified", "Email is already verified.");

        IsEmailVerified = true;
        Touch();

        RaiseDomainEvent(new UserEmailVerifiedDomainEvent(Guid.NewGuid(), DateTime.UtcNow, Id));
    }

    public void UpdateSlug(string slugValue)
    {
        Slug = Slug.Create(slugValue);
        Touch();
    }

    public void TogglePublicProfile()
    {
        IsPublicProfile = !IsPublicProfile;
        Touch();
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw DomainException.Create("User.PasswordHashEmpty", "Password hash cannot be empty.");

        PasswordHash = newPasswordHash;
        Touch();
    }

    public void PromoteToAdmin()
    {
        Role = UserRole.Admin;
        Touch();
    }
}
