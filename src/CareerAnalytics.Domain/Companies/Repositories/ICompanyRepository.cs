using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Companies.Repositories;

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Company>> SearchAsync(string term, CancellationToken cancellationToken = default);
}
