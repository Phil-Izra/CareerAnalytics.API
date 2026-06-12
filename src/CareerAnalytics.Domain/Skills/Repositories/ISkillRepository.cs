using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Skills.Repositories;

public interface ISkillRepository : IRepository<Skill>
{
    Task<Skill?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Skill>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserSkill>> GetUserSkillsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddUserSkillAsync(UserSkill userSkill, CancellationToken cancellationToken = default);
    Task UpdateUserSkillAsync(UserSkill userSkill, CancellationToken cancellationToken = default);
}
