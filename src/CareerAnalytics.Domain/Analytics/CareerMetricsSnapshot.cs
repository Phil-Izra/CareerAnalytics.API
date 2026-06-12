using CareerAnalytics.Domain.Analytics.ValueObjects;
using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Analytics;

/// <summary>
/// Pre-computed score snapshot for a user at a given period.
/// Drives the Career Performance Dashboard graph (X=time, Y=Career Value Score).
/// </summary>
public sealed class CareerMetricsSnapshot : BaseEntity
{
    public Guid UserId { get; private set; }
    public PeriodType PeriodType { get; private set; }

    /// <summary>First day of the period (month or year) this snapshot represents.</summary>
    public DateTime PeriodDate { get; private set; }

    public int TotalScore { get; private set; }
    public int TechnicalScore { get; private set; }
    public int LeadershipScore { get; private set; }
    public int BusinessImpactScore { get; private set; }

    private CareerMetricsSnapshot() { }

    public static CareerMetricsSnapshot Create(
        Guid userId,
        PeriodType periodType,
        DateTime periodDate,
        int totalScore,
        int technicalScore,
        int leadershipScore,
        int businessImpactScore)
    {
        if (userId == Guid.Empty)
            throw DomainException.Create("Snapshot.InvalidUserId", "UserId cannot be empty.");

        return new CareerMetricsSnapshot
        {
            UserId = userId,
            PeriodType = periodType,
            PeriodDate = periodDate.Date,
            TotalScore = totalScore,
            TechnicalScore = technicalScore,
            LeadershipScore = leadershipScore,
            BusinessImpactScore = businessImpactScore
        };
    }
}
