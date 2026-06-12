using CareerAnalytics.Domain.CareerEvents.Entities;
using CareerAnalytics.Domain.CareerEvents.Enums;
using CareerAnalytics.Domain.CareerEvents.Events;
using CareerAnalytics.Domain.CareerEvents.ValueObjects;
using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.CareerEvents;

public sealed class CareerEvent : AggregateRoot
{
    private readonly List<EventAchievement> _achievements = [];
    private readonly List<EventSkill> _skills = [];

    public Guid UserId { get; private set; }
    public Guid? CompanyId { get; private set; }
    public string Title { get; private set; } = null!;
    public string? ShortDescription { get; private set; }
    public EventType EventType { get; private set; }
    public ImpactScore ImpactScore { get; private set; } = null!;
    public DateRange Period { get; private set; } = null!;
    public bool IsPublic { get; private set; }

    public IReadOnlyCollection<EventAchievement> Achievements => _achievements.AsReadOnly();
    public IReadOnlyCollection<EventSkill> Skills => _skills.AsReadOnly();

    private CareerEvent() { }

    public static CareerEvent Create(
        Guid userId,
        string title,
        EventType eventType,
        DateTime startDate,
        DateTime? endDate = null,
        Guid? companyId = null,
        string? shortDescription = null,
        int? customImpactScore = null,
        bool isPublic = true)
    {
        if (userId == Guid.Empty)
            throw DomainException.Create("CareerEvent.InvalidUserId", "UserId cannot be empty.");

        if (string.IsNullOrWhiteSpace(title))
            throw DomainException.Create("CareerEvent.TitleEmpty", "Career event title cannot be empty.");

        var score = customImpactScore.HasValue
            ? ImpactScore.Create(customImpactScore.Value)
            : ImpactScore.Default(eventType);

        var careerEvent = new CareerEvent
        {
            UserId = userId,
            CompanyId = companyId,
            Title = title.Trim(),
            ShortDescription = shortDescription?.Trim(),
            EventType = eventType,
            ImpactScore = score,
            Period = DateRange.Create(startDate, endDate),
            IsPublic = isPublic
        };

        careerEvent.RaiseDomainEvent(new CareerEventCreatedDomainEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            careerEvent.Id,
            userId,
            eventType,
            score.Value));

        return careerEvent;
    }

    public void AddAchievement(
        string title,
        string? description = null,
        decimal? metricValue = null,
        string? metricUnit = null,
        string? evidenceUrl = null,
        EvidenceType? evidenceType = null)
    {
        var achievement = EventAchievement.Create(
            Id, title, description, metricValue, metricUnit, evidenceUrl, evidenceType,
            _achievements.Count);

        _achievements.Add(achievement);
        Touch();
    }

    public void AttachSkill(Guid skillId)
    {
        if (_skills.Any(s => s.SkillId == skillId))
            return;

        _skills.Add(EventSkill.Create(Id, skillId));
        Touch();
    }

    public void DetachSkill(Guid skillId)
    {
        var skill = _skills.FirstOrDefault(s => s.SkillId == skillId);
        if (skill is null) return;

        _skills.Remove(skill);
        Touch();
    }

    public void AdjustScore(int newScore)
    {
        var old = ImpactScore.Value;
        ImpactScore = ImpactScore.Create(newScore);
        Touch();

        RaiseDomainEvent(new CareerEventScoreAdjustedDomainEvent(
            Guid.NewGuid(), DateTime.UtcNow, Id, UserId, old, newScore));
    }

    public void UpdatePeriod(DateTime start, DateTime? end)
    {
        Period = DateRange.Create(start, end);
        Touch();
    }

    public void ToggleVisibility()
    {
        IsPublic = !IsPublic;
        Touch();
    }
}
