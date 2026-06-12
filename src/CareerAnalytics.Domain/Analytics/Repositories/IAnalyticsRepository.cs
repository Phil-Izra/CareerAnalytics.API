using CareerAnalytics.Domain.Analytics.ValueObjects;

namespace CareerAnalytics.Domain.Analytics.Repositories;

public interface IAnalyticsRepository
{
    Task<IReadOnlyList<CareerMetricsSnapshot>> GetSnapshotsByUserIdAsync(
        Guid userId,
        PeriodType periodType,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CareerMetricsSnapshot>> GetSnapshotsByUserIdInRangeAsync(
        Guid userId,
        PeriodType periodType,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default);

    Task AddSnapshotAsync(CareerMetricsSnapshot snapshot, CancellationToken cancellationToken = default);
    Task UpsertSnapshotAsync(CareerMetricsSnapshot snapshot, CancellationToken cancellationToken = default);

    Task RecordProfileViewAsync(ProfileView profileView, CancellationToken cancellationToken = default);
    Task<int> GetProfileViewCountAsync(Guid userId, CancellationToken cancellationToken = default);
}
