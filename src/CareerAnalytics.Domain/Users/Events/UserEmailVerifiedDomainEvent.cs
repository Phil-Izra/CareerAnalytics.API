using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Users.Events;

public sealed record UserEmailVerifiedDomainEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid UserId) : IDomainEvent;
