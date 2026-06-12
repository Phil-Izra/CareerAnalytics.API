using CareerAnalytics.Application.Common.Models;
using CareerAnalytics.Domain.CareerEvents.Enums;
using MediatR;

namespace CareerAnalytics.Application.CareerEvents.Commands.CreateCareerEvent;

public sealed record CreateCareerEventCommand(
    Guid UserId,
    string Title,
    EventType EventType,
    DateTime StartDate,
    DateTime? EndDate,
    Guid? CompanyId,
    string? ShortDescription,
    int? CustomImpactScore,
    bool IsPublic = true) : IRequest<Result<Guid>>;
