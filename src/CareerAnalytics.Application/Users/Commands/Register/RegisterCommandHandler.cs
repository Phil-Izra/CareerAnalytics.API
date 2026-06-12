using CareerAnalytics.Application.Common.Interfaces;
using CareerAnalytics.Application.Common.Models;
using CareerAnalytics.Domain.Common;
using CareerAnalytics.Domain.Profiles;
using CareerAnalytics.Domain.Profiles.Repositories;
using CareerAnalytics.Domain.Users;
using CareerAnalytics.Domain.Users.Repositories;
using MediatR;

namespace CareerAnalytics.Application.Users.Commands.Register;

public sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IProfileRepository profileRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return Result.Failure<RegisterResponse>("User.EmailTaken", "An account with this email already exists.");

        string passwordHash;
        try
        {
            passwordHash = passwordHasher.Hash(request.Password);
        }
        catch (Exception ex)
        {
            return Result.Failure<RegisterResponse>("User.HashFailed", ex.Message);
        }

        User user;
        try
        {
            user = User.Create(request.FullName, request.Email, passwordHash);
        }
        catch (DomainException ex)
        {
            return Result.Failure<RegisterResponse>(ex.Code, ex.Message);
        }

        // Ensure slug uniqueness by appending a suffix when taken
        var slug = user.Slug.Value;
        if (await userRepository.ExistsBySlugAsync(slug, cancellationToken))
        {
            var unique = $"{slug}-{Guid.NewGuid().ToString()[..6]}";
            user.UpdateSlug(unique);
        }

        await userRepository.AddAsync(user, cancellationToken);

        var profile = Profile.Create(user.Id);
        await profileRepository.AddAsync(profile, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new RegisterResponse(user.Id, user.Slug.Value, user.Email.Value));
    }
}
