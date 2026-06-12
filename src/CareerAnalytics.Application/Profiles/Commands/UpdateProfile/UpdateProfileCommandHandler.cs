using CareerAnalytics.Application.Common.Interfaces;
using CareerAnalytics.Application.Common.Models;
using CareerAnalytics.Domain.Common;
using CareerAnalytics.Domain.Profiles.Repositories;
using MediatR;

namespace CareerAnalytics.Application.Profiles.Commands.UpdateProfile;

public sealed class UpdateProfileCommandHandler(
    IProfileRepository profileRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateProfileCommand, Result>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (profile is null)
            return Result.Failure("Profile.NotFound", "Profile not found.");

        try
        {
            profile.Update(
                request.Headline,
                request.Summary,
                request.Location,
                request.AvatarUrl,
                request.LinkedInUrl,
                request.GitHubUrl,
                request.PortfolioUrl,
                request.Theme);
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Code, ex.Message);
        }

        await profileRepository.UpdateAsync(profile, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
