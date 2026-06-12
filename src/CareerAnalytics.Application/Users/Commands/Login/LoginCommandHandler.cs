using CareerAnalytics.Application.Common.Interfaces;
using CareerAnalytics.Application.Common.Models;
using CareerAnalytics.Domain.Users.Repositories;
using MediatR;

namespace CareerAnalytics.Application.Users.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Failure<LoginResponse>("Auth.InvalidCredentials", "Email or password is incorrect.");

        if (!user.IsEmailVerified)
            return Result.Failure<LoginResponse>("Auth.EmailNotVerified", "Please verify your email address before logging in.");

        var token = jwtTokenService.GenerateToken(user);

        return Result.Success(new LoginResponse(token, user.Id, user.Slug.Value, user.FullName));
    }
}
