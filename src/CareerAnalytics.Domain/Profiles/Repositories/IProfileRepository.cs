using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Profiles.Repositories;

public interface IProfileRepository : IRepository<Profile>
{
    Task<Profile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
