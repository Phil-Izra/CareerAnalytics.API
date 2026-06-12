using CareerAnalytics.Domain.CareerEvents.Enums;
using CareerAnalytics.Domain.Common;

namespace CareerAnalytics.Domain.CareerEvents.Events;

public sealed record CareerEventCreatedDomainEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid CareerEventId,
    Guid UserId,
    EventType EventType,
    int ImpactScore) : IDomainEvent;
