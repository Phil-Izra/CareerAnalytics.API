using CareerAnalytics.Domain.CareerEvents.Enums;

namespace CareerAnalytics.Application.CareerEvents.DTOs;

public sealed record CareerEventDto(
    Guid Id,
    string Title,
    string? ShortDescription,
    EventType EventType,
    string EventTypeLabel,
    int ImpactScore,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsOngoing,
    string? CompanyName,
    string? CompanyLogoUrl,
    IReadOnlyList<AchievementDto> Achievements,
    IReadOnlyList<string> SkillNames);

public sealed record AchievementDto(
    Guid Id,
    string Title,
    string? Description,
    decimal? MetricValue,
    string? MetricUnit,
    string? EvidenceUrl,
    string? EvidenceType);
