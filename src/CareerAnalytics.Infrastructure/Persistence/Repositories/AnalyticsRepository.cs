using CareerAnalytics.Domain.Analytics;
using CareerAnalytics.Domain.Analytics.Repositories;
using CareerAnalytics.Domain.Analytics.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CareerAnalytics.Infrastructure.Persistence.Repositories;

public sealed class AnalyticsRepository(ApplicationDbContext dbContext) : IAnalyticsRepository
{
    public async Task<IReadOnlyList<CareerMetricsSnapshot>> GetSnapshotsByUserIdAsync(
        Guid userId, PeriodType periodType, CancellationToken cancellationToken = default) =>
        await dbContext.CareerMetricsSnapshots
            .Where(s => s.UserId == userId && s.PeriodType == periodType)
            .OrderBy(s => s.PeriodDate)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CareerMetricsSnapshot>> GetSnapshotsByUserIdInRangeAsync(
        Guid userId, PeriodType periodType, DateTime from, DateTime to,
        CancellationToken cancellationToken = default) =>
        await dbContext.CareerMetricsSnapshots
            .Where(s => s.UserId == userId && s.PeriodType == periodType
                        && s.PeriodDate >= from && s.PeriodDate <= to)
            .OrderBy(s => s.PeriodDate)
            .ToListAsync(cancellationToken);

    public async Task AddSnapshotAsync(CareerMetricsSnapshot snapshot, CancellationToken cancellationToken = default) =>
        await dbContext.CareerMetricsSnapshots.AddAsync(snapshot, cancellationToken);

    public async Task UpsertSnapshotAsync(CareerMetricsSnapshot snapshot, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.CareerMetricsSnapshots
            .FirstOrDefaultAsync(s =>
                s.UserId == snapshot.UserId &&
                s.PeriodType == snapshot.PeriodType &&
                s.PeriodDate == snapshot.PeriodDate, cancellationToken);

        if (existing is null)
            await dbContext.CareerMetricsSnapshots.AddAsync(snapshot, cancellationToken);
        else
            dbContext.CareerMetricsSnapshots.Remove(existing);
    }

    public async Task RecordProfileViewAsync(ProfileView profileView, CancellationToken cancellationToken = default) =>
        await dbContext.ProfileViews.AddAsync(profileView, cancellationToken);

    public async Task<int> GetProfileViewCountAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await dbContext.ProfileViews.CountAsync(v => v.ProfileUserId == userId, cancellationToken);
}
