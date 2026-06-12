using CareerAnalytics.Application.CareerEvents.DTOs;
using CareerAnalytics.Domain.Analytics.ValueObjects;

namespace CareerAnalytics.Application.CareerEvents.Queries.GetPublicDashboard;

public sealed record PublicDashboardDto(
    // Profile header
    string FullName,
    string Slug,
    string? Headline,
    string? Summary,
    string? Location,
    string? AvatarUrl,
    string? LinkedInUrl,
    string? GitHubUrl,
    string? PortfolioUrl,
    string ProfileTheme,

    // Career metrics panel
    int TotalYearsExperience,
    int TotalCareerScore,
    int PromotionCount,
    int CertificationCount,
    int ProjectCount,
    int LeadershipCount,
    IReadOnlyList<string> Industries,

    // Career graph data (X=time, Y=score)
    IReadOnlyList<ScoreDataPoint> ScoreTimeline,

    // Clickable timeline events
    IReadOnlyList<CareerEventDto> Timeline,

    // Skill evolution
    IReadOnlyList<SkillEvolutionEntry> SkillEvolution,

    // Profile views
    int ProfileViewCount);

public sealed record ScoreDataPoint(DateTime PeriodDate, int Score, PeriodType PeriodType);

public sealed record SkillEvolutionEntry(
    string SkillName,
    string Category,
    IReadOnlyList<SkillPeriodSnapshot> Periods);

public sealed record SkillPeriodSnapshot(DateTime Date, string EventTitle, int ImpactScore);
