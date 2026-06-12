using CareerAnalytics.Domain.Common;
using CareerAnalytics.Domain.Skills.Enums;

namespace CareerAnalytics.Domain.Skills;

public sealed class Skill : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public SkillCategory Category { get; private set; }

    private Skill() { }

    public static Skill Create(string name, SkillCategory category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw DomainException.Create("Skill.NameEmpty", "Skill name cannot be empty.");

        return new Skill
        {
            Name = name.Trim(),
            Category = category
        };
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw DomainException.Create("Skill.NameEmpty", "Skill name cannot be empty.");

        Name = name.Trim();
        Touch();
    }

    public void ChangeCategory(SkillCategory category)
    {
        Category = category;
        Touch();
    }
}
