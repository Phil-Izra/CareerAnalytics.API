using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Analytics;

public sealed class ProfileView : BaseEntity
{
    public Guid ProfileUserId { get; private set; }

    /// <summary>SHA-256 hash of the viewer's IP — stored hashed for GDPR compliance.</summary>
    public string ViewerIpHash { get; private set; } = null!;

    public string? Country { get; private set; }
    public string? SessionId { get; private set; }
    public DateTime ViewedAt { get; private set; }

    private ProfileView() { }

    public static ProfileView Record(
        Guid profileUserId,
        string viewerIpHash,
        string? country = null,
        string? sessionId = null)
    {
        if (profileUserId == Guid.Empty)
            throw DomainException.Create("ProfileView.InvalidUserId", "ProfileUserId cannot be empty.");

        if (string.IsNullOrWhiteSpace(viewerIpHash))
            throw DomainException.Create("ProfileView.IpHashEmpty", "Viewer IP hash cannot be empty.");

        return new ProfileView
        {
            ProfileUserId = profileUserId,
            ViewerIpHash = viewerIpHash,
            Country = country,
            SessionId = sessionId,
            ViewedAt = DateTime.UtcNow
        };
    }
}
