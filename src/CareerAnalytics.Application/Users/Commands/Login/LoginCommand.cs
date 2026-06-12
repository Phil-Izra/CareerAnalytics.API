using CareerAnalytics.Application.Common.Models;
using MediatR;

namespace CareerAnalytics.Application.Users.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

public sealed record LoginResponse(string Token, Guid UserId, string Slug, string FullName);
