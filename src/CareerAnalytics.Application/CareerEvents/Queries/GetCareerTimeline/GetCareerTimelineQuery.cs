using CareerAnalytics.Application.CareerEvents.DTOs;
using CareerAnalytics.Application.Common.Models;
using MediatR;

namespace CareerAnalytics.Application.CareerEvents.Queries.GetCareerTimeline;

public sealed record GetCareerTimelineQuery(Guid UserId, bool PublicOnly = true)
    : IRequest<Result<IReadOnlyList<CareerEventDto>>>;
