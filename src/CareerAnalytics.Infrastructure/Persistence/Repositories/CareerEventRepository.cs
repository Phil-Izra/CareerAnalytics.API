using CareerAnalytics.Domain.CareerEvents;
using CareerAnalytics.Domain.CareerEvents.Enums;
using CareerAnalytics.Domain.CareerEvents.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CareerAnalytics.Infrastructure.Persistence.Repositories;

public sealed class CareerEventRepository(ApplicationDbContext dbContext)
    : BaseRepository<CareerEvent>(dbContext), ICareerEventRepository
{
    public async Task<IReadOnlyList<CareerEvent>> GetByUserIdAsync(
        Guid userId, CancellationToken cancellationToken = default) =>
        await DbContext.CareerEvents
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Period.Start)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CareerEvent>> GetPublicByUserIdAsync(
        Guid userId, CancellationToken cancellationToken = default) =>
        await DbContext.CareerEvents
            .Where(e => e.UserId == userId && e.IsPublic)
            .OrderBy(e => e.Period.Start)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CareerEvent>> GetByUserIdAndTypeAsync(
        Guid userId, EventType eventType, CancellationToken cancellationToken = default) =>
        await DbContext.CareerEvents
            .Where(e => e.UserId == userId && e.EventType == eventType)
            .OrderBy(e => e.Period.Start)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CareerEvent>> GetByUserIdInPeriodAsync(
        Guid userId, DateTime from, DateTime to, CancellationToken cancellationToken = default) =>
        await DbContext.CareerEvents
            .Where(e => e.UserId == userId && e.Period.Start >= from && e.Period.Start <= to)
            .OrderBy(e => e.Period.Start)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CareerEvent>> GetWithSkillsByUserIdAsync(
        Guid userId, CancellationToken cancellationToken = default) =>
        await DbContext.CareerEvents
            .Where(e => e.UserId == userId)
            .Include("_achievements")
            .Include("_skills")
            .OrderBy(e => e.Period.Start)
            .ToListAsync(cancellationToken);
}
