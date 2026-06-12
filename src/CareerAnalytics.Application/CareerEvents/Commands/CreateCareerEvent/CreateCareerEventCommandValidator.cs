using FluentValidation;

namespace CareerAnalytics.Application.CareerEvents.Commands.CreateCareerEvent;

public sealed class CreateCareerEventCommandValidator : AbstractValidator<CreateCareerEventCommand>
{
    public CreateCareerEventCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EventType).IsInEnum();
        RuleFor(x => x.StartDate).LessThanOrEqualTo(DateTime.UtcNow.AddDays(1));
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.EndDate.HasValue);
        RuleFor(x => x.CustomImpactScore)
            .InclusiveBetween(0, 100)
            .When(x => x.CustomImpactScore.HasValue);
        RuleFor(x => x.ShortDescription).MaximumLength(500).When(x => x.ShortDescription is not null);
    }
}
