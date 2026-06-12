using CareerAnalytics.Domain.Skills;
using CareerAnalytics.Domain.Skills.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CareerAnalytics.Infrastructure.Persistence.Repositories;

public sealed class SkillRepository(ApplicationDbContext dbContext)
    : BaseRepository<Skill>(dbContext), ISkillRepository
{
    public async Task<Skill?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await DbContext.Skills
            .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);

    public async Task<IReadOnlyList<Skill>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbContext.Skills.OrderBy(s => s.Name).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<UserSkill>> GetUserSkillsAsync(
        Guid userId, CancellationToken cancellationToken = default) =>
        await DbContext.UserSkills
            .Where(us => us.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task AddUserSkillAsync(UserSkill userSkill, CancellationToken cancellationToken = default) =>
        await DbContext.UserSkills.AddAsync(userSkill, cancellationToken);

    public Task UpdateUserSkillAsync(UserSkill userSkill, CancellationToken cancellationToken = default)
    {
        DbContext.UserSkills.Update(userSkill);
        return Task.CompletedTask;
    }
}
