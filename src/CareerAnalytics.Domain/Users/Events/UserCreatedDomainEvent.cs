using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid UserId,
    string Email,
    string Slug) : IDomainEvent;
