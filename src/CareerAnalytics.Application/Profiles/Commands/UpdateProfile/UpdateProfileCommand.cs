using CareerAnalytics.Application.Common.Models;
using MediatR;

namespace CareerAnalytics.Application.Profiles.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(
    Guid UserId,
    string? Headline,
    string? Summary,
    string? Location,
    string? AvatarUrl,
    string? LinkedInUrl,
    string? GitHubUrl,
    string? PortfolioUrl,
    string? Theme) : IRequest<Result>;
