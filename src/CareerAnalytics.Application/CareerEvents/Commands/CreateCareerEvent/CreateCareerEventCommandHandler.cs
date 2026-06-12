using CareerAnalytics.Application.Common.Interfaces;
using CareerAnalytics.Application.Common.Models;
using CareerAnalytics.Domain.CareerEvents;
using CareerAnalytics.Domain.CareerEvents.Repositories;
using CareerAnalytics.Domain.Common;
using MediatR;

namespace CareerAnalytics.Application.CareerEvents.Commands.CreateCareerEvent;

public sealed class CreateCareerEventCommandHandler(
    ICareerEventRepository careerEventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCareerEventCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateCareerEventCommand request,
        CancellationToken cancellationToken)
    {
        CareerEvent careerEvent;
        try
        {
            careerEvent = CareerEvent.Create(
                request.UserId,
                request.Title,
                request.EventType,
                request.StartDate,
                request.EndDate,
                request.CompanyId,
                request.ShortDescription,
                request.CustomImpactScore,
                request.IsPublic);
        }
        catch (DomainException ex)
        {
            return Result.Failure<Guid>(ex.Code, ex.Message);
        }

        await careerEventRepository.AddAsync(careerEvent, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(careerEvent.Id);
    }
}
