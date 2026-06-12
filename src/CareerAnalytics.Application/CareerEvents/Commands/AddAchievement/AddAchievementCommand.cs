using CareerAnalytics.Application.Common.Models;
using CareerAnalytics.Domain.CareerEvents.Entities;
using MediatR;

namespace CareerAnalytics.Application.CareerEvents.Commands.AddAchievement;

public sealed record AddAchievementCommand(
    Guid CareerEventId,
    Guid RequestingUserId,
    string Title,
    string? Description,
    decimal? MetricValue,
    string? MetricUnit,
    string? EvidenceUrl,
    EvidenceType? EvidenceType) : IRequest<Result>;
