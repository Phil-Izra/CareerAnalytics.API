using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.CareerEvents.Events;

public sealed record CareerEventScoreAdjustedDomainEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid CareerEventId,
    Guid UserId,
    int OldScore,
    int NewScore) : IDomainEvent;
