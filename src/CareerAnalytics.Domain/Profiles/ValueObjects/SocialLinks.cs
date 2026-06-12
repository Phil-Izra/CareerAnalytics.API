using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Profiles.ValueObjects;

public sealed class SocialLinks : ValueObject
{
    public string? LinkedInUrl { get; }
    public string? GitHubUrl { get; }
    public string? PortfolioUrl { get; }

    private SocialLinks(string? linkedInUrl, string? gitHubUrl, string? portfolioUrl)
    {
        LinkedInUrl = linkedInUrl;
        GitHubUrl = gitHubUrl;
        PortfolioUrl = portfolioUrl;
    }

    public static SocialLinks Create(string? linkedInUrl, string? gitHubUrl, string? portfolioUrl)
    {
        if (linkedInUrl is not null && !linkedInUrl.Contains("linkedin.com"))
            throw DomainException.Create("SocialLinks.InvalidLinkedIn", "LinkedIn URL must be a linkedin.com URL.");

        if (gitHubUrl is not null && !gitHubUrl.Contains("github.com"))
            throw DomainException.Create("SocialLinks.InvalidGitHub", "GitHub URL must be a github.com URL.");

        return new SocialLinks(
            linkedInUrl?.Trim(),
            gitHubUrl?.Trim(),
            portfolioUrl?.Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return LinkedInUrl ?? string.Empty;
        yield return GitHubUrl ?? string.Empty;
        yield return PortfolioUrl ?? string.Empty;
    }
}
