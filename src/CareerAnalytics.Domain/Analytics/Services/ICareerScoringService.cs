namespace CareerAnalytics.Domain.Analytics.Services;

/// <summary>
/// Domain service: computes career performance scores from a user's events.
/// Lives in the domain because the scoring formula is core business logic.
/// </summary>
public interface ICareerScoringService
{
    /// <summary>
    /// Recomputes and persists metric snapshots for a user after their events change.
    /// </summary>
    Task RecalculateAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the current total career value score for a user.
    /// </summary>
    Task<int> GetCurrentScoreAsync(Guid userId, CancellationToken cancellationToken = default);
}
