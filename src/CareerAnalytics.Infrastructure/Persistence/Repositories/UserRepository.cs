using CareerAnalytics.Domain.Users;
using CareerAnalytics.Domain.Users.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CareerAnalytics.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext)
    : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await DbContext.Users
            .FirstOrDefaultAsync(u => u.Email.Value == email.ToLowerInvariant(), cancellationToken);

    public async Task<User?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default) =>
        await DbContext.Users
            .FirstOrDefaultAsync(u => u.Slug.Value == slug, cancellationToken);

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await DbContext.Users
            .AnyAsync(u => u.Email.Value == email.ToLowerInvariant(), cancellationToken);

    public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default) =>
        await DbContext.Users
            .AnyAsync(u => u.Slug.Value == slug, cancellationToken);
}
