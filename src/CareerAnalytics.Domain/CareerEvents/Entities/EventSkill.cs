using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.CareerEvents.Entities;

/// <summary>
/// Associates a skill with a career event, enabling tech-stack-per-role tracking
/// and powering the Skill Evolution Chart.
/// </summary>
public sealed class EventSkill : BaseEntity
{
    public Guid CareerEventId { get; private set; }
    public Guid SkillId { get; private set; }

    private EventSkill() { }

    public static EventSkill Create(Guid careerEventId, Guid skillId)
    {
        if (careerEventId == Guid.Empty)
            throw DomainException.Create("EventSkill.InvalidCareerEventId", "CareerEventId cannot be empty.");

        if (skillId == Guid.Empty)
            throw DomainException.Create("EventSkill.InvalidSkillId", "SkillId cannot be empty.");

        return new EventSkill
        {
            CareerEventId = careerEventId,
            SkillId = skillId
        };
    }
}
