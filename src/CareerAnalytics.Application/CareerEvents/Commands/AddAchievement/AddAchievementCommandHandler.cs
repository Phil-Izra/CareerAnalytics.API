using CareerAnalytics.Application.Common.Interfaces;
using CareerAnalytics.Application.Common.Models;
using CareerAnalytics.Domain.CareerEvents.Repositories;
using CareerAnalytics.Domain.Common;
using MediatR;

namespace CareerAnalytics.Application.CareerEvents.Commands.AddAchievement;

public sealed class AddAchievementCommandHandler(
    ICareerEventRepository careerEventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddAchievementCommand, Result>
{
    public async Task<Result> Handle(AddAchievementCommand request, CancellationToken cancellationToken)
    {
        var careerEvent = await careerEventRepository.GetByIdAsync(request.CareerEventId, cancellationToken);
        if (careerEvent is null)
            return Result.Failure("CareerEvent.NotFound", "Career event not found.");

        if (careerEvent.UserId != request.RequestingUserId)
            return Result.Failure("CareerEvent.Forbidden", "You do not own this career event.");

        try
        {
            careerEvent.AddAchievement(
                request.Title,
                request.Description,
                request.MetricValue,
                request.MetricUnit,
                request.EvidenceUrl,
                request.EvidenceType);
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Code, ex.Message);
        }

        await careerEventRepository.UpdateAsync(careerEvent, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
