using CareerAnalytics.Domain.Common;
using CareerAnalytics.Domain.Profiles.ValueObjects;

namespace CareerAnalytics.Domain.Profiles;

public sealed class Profile : AggregateRoot
{
    public Guid UserId { get; private set; }
    public string? Headline { get; private set; }
    public string? Summary { get; private set; }
    public string? Location { get; private set; }
    public string? AvatarUrl { get; private set; }

    /// <summary>
    /// Cached denormalized value. Authoritative value is derived from CareerEvents.
    /// </summary>
    public int YearsOfExperience { get; private set; }

    public SocialLinks SocialLinks { get; private set; } = null!;
    public ProfileTheme Theme { get; private set; } = null!;

    private Profile() { }

    public static Profile Create(Guid userId)
    {
        if (userId == Guid.Empty)
            throw DomainException.Create("Profile.InvalidUserId", "UserId cannot be empty.");

        return new Profile
        {
            UserId = userId,
            SocialLinks = SocialLinks.Create(null, null, null),
            Theme = ProfileTheme.Default
        };
    }

    public void Update(
        string? headline,
        string? summary,
        string? location,
        string? avatarUrl,
        string? linkedInUrl,
        string? gitHubUrl,
        string? portfolioUrl,
        string? theme)
    {
        Headline = headline?.Trim();
        Summary = summary?.Trim();
        Location = location?.Trim();
        AvatarUrl = avatarUrl?.Trim();
        SocialLinks = SocialLinks.Create(linkedInUrl, gitHubUrl, portfolioUrl);

        if (theme is not null)
            Theme = ProfileTheme.From(theme);

        Touch();
    }

    public void UpdateYearsOfExperience(int years)
    {
        if (years < 0 || years > 60)
            throw DomainException.Create("Profile.InvalidYearsOfExperience", "Years of experience must be between 0 and 60.");

        YearsOfExperience = years;
        Touch();
    }
}
