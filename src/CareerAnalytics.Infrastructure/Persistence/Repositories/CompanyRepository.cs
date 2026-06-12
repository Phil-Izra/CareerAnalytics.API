using CareerAnalytics.Domain.Companies;
using CareerAnalytics.Domain.Companies.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CareerAnalytics.Infrastructure.Persistence.Repositories;

public sealed class CompanyRepository(ApplicationDbContext dbContext)
    : BaseRepository<Company>(dbContext), ICompanyRepository
{
    public async Task<Company?> GetByNameAsync(string name, CancellationToken cancellationToken = default) =>
        await DbContext.Companies
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);

    public async Task<IReadOnlyList<Company>> SearchAsync(string term, CancellationToken cancellationToken = default) =>
        await DbContext.Companies
            .Where(c => EF.Functions.ILike(c.Name, $"%{term}%"))
            .OrderBy(c => c.Name)
            .Take(20)
            .ToListAsync(cancellationToken);
}
