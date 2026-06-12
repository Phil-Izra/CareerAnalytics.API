using CareerAnalytics.Domain.Common;
using CareerAnalytics.Domain.Skills.ValueObjects;

namespace CareerAnalytics.Domain.Skills;

/// <summary>
/// Summary of a user's proficiency in a skill.
/// The Skill Evolution Chart is driven by EventSkills over time; this provides the current snapshot.
/// </summary>
public sealed class UserSkill : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid SkillId { get; private set; }
    public ProficiencyLevel ProficiencyLevel { get; private set; } = null!;
    public DateTime FirstUsedDate { get; private set; }
    public DateTime LastUsedDate { get; private set; }

    private UserSkill() { }

    public static UserSkill Create(Guid userId, Guid skillId, int proficiencyLevel, DateTime firstUsedDate)
    {
        if (userId == Guid.Empty)
            throw DomainException.Create("UserSkill.InvalidUserId", "UserId cannot be empty.");

        if (skillId == Guid.Empty)
            throw DomainException.Create("UserSkill.InvalidSkillId", "SkillId cannot be empty.");

        return new UserSkill
        {
            UserId = userId,
            SkillId = skillId,
            ProficiencyLevel = ProficiencyLevel.Create(proficiencyLevel),
            FirstUsedDate = firstUsedDate,
            LastUsedDate = firstUsedDate
        };
    }

    public void UpdateProficiency(int level, DateTime lastUsed)
    {
        ProficiencyLevel = ProficiencyLevel.Create(level);
        if (lastUsed > LastUsedDate) LastUsedDate = lastUsed;
        Touch();
    }
}
