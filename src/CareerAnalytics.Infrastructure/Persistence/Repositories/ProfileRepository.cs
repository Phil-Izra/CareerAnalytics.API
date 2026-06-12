using CareerAnalytics.Domain.Profiles;
using CareerAnalytics.Domain.Profiles.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CareerAnalytics.Infrastructure.Persistence.Repositories;

public sealed class ProfileRepository(ApplicationDbContext dbContext)
    : BaseRepository<Profile>(dbContext), IProfileRepository
{
    public async Task<Profile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await DbContext.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
}
