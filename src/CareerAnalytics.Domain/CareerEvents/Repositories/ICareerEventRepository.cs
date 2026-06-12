using CareerAnalytics.Domain.CareerEvents.Enums;
using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.CareerEvents.Repositories;

public interface ICareerEventRepository : IRepository<CareerEvent>
{
    Task<IReadOnlyList<CareerEvent>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CareerEvent>> GetPublicByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CareerEvent>> GetByUserIdAndTypeAsync(Guid userId, EventType eventType, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CareerEvent>> GetByUserIdInPeriodAsync(Guid userId, DateTime from, DateTime to, CancellationToken cancellationToken = default);

    /// <summary>Returns events with their associated skills for the Skill Evolution Chart.</summary>
    Task<IReadOnlyList<CareerEvent>> GetWithSkillsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
