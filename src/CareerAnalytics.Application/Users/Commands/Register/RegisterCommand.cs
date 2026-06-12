using MediatR;
using CareerAnalytics.Application.Common.Models;

namespace CareerAnalytics.Application.Users.Commands.Register;

public sealed record RegisterCommand(
    string FullName,
    string Email,
    string Password) : IRequest<Result<RegisterResponse>>;

public sealed record RegisterResponse(Guid UserId, string Slug, string Email);
