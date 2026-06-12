using CareerAnalytics.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CareerAnalytics.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T>(ApplicationDbContext dbContext) : IRepository<T>
    where T : AggregateRoot
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await DbContext.Set<T>().FindAsync([id], cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await DbContext.Set<T>().AddAsync(entity, cancellationToken);

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
}
