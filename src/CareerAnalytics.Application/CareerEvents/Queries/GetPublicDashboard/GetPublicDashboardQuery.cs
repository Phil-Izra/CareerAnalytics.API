using CareerAnalytics.Application.Common.Models;
using MediatR;

namespace CareerAnalytics.Application.CareerEvents.Queries.GetPublicDashboard;

public sealed record GetPublicDashboardQuery(string Slug) : IRequest<Result<PublicDashboardDto>>;
