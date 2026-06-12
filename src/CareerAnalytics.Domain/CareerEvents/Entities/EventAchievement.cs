using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.CareerEvents.Entities;

/// <summary>
/// Represents a measurable result or piece of evidence for a career event.
/// Drives the recruiter drill-down popup: metrics, screenshots, GitHub links.
/// </summary>
public sealed class EventAchievement : BaseEntity
{
    public Guid CareerEventId { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }

    /// <summary>Numeric value of the metric, e.g. 40 for "reduced latency by 40%".</summary>
    public decimal? MetricValue { get; private set; }
    public string? MetricUnit { get; private set; }

    public string? EvidenceUrl { get; private set; }
    public EvidenceType? EvidenceType { get; private set; }
    public int DisplayOrder { get; private set; }

    private EventAchievement() { }

    public static EventAchievement Create(
        Guid careerEventId,
        string title,
        string? description = null,
        decimal? metricValue = null,
        string? metricUnit = null,
        string? evidenceUrl = null,
        EvidenceType? evidenceType = null,
        int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw DomainException.Create("EventAchievement.TitleEmpty", "Achievement title cannot be empty.");

        return new EventAchievement
        {
            CareerEventId = careerEventId,
            Title = title.Trim(),
            Description = description?.Trim(),
            MetricValue = metricValue,
            MetricUnit = metricUnit?.Trim(),
            EvidenceUrl = evidenceUrl?.Trim(),
            EvidenceType = evidenceType,
            DisplayOrder = displayOrder
        };
    }
}

public enum EvidenceType
{
    GitHubLink = 1,
    Screenshot = 2,
    Article = 3,
    Certificate = 4,
    Video = 5,
    Other = 99
}
