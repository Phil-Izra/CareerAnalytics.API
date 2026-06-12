using CareerAnalytics.Application.CareerEvents.DTOs;
using CareerAnalytics.Application.Common.Models;
using CareerAnalytics.Domain.CareerEvents;
using CareerAnalytics.Domain.CareerEvents.Repositories;
using CareerAnalytics.Domain.Companies.Repositories;
using CareerAnalytics.Domain.Skills.Repositories;
using MediatR;

namespace CareerAnalytics.Application.CareerEvents.Queries.GetCareerTimeline;

public sealed class GetCareerTimelineQueryHandler(
    ICareerEventRepository careerEventRepository,
    ICompanyRepository companyRepository,
    ISkillRepository skillRepository) : IRequestHandler<GetCareerTimelineQuery, Result<IReadOnlyList<CareerEventDto>>>
{
    public async Task<Result<IReadOnlyList<CareerEventDto>>> Handle(
        GetCareerTimelineQuery request,
        CancellationToken cancellationToken)
    {
        var events = request.PublicOnly
            ? await careerEventRepository.GetPublicByUserIdAsync(request.UserId, cancellationToken)
            : await careerEventRepository.GetWithSkillsByUserIdAsync(request.UserId, cancellationToken);

        var allSkills = await skillRepository.GetAllAsync(cancellationToken);
        var skillMap = allSkills.ToDictionary(s => s.Id, s => s.Name);

        var dtos = new List<CareerEventDto>(events.Count);
        foreach (var e in events.OrderBy(e => e.Period.Start))
        {
            string? companyName = null;
            string? companyLogoUrl = null;
            if (e.CompanyId.HasValue)
            {
                var company = await companyRepository.GetByIdAsync(e.CompanyId.Value, cancellationToken);
                companyName = company?.Name;
                companyLogoUrl = company?.LogoUrl;
            }

            dtos.Add(MapToDto(e, companyName, companyLogoUrl, skillMap));
        }

        return Result.Success<IReadOnlyList<CareerEventDto>>(dtos);
    }

    private static CareerEventDto MapToDto(
        CareerEvent e,
        string? companyName,
        string? companyLogoUrl,
        Dictionary<Guid, string> skillMap) =>
        new(
            e.Id,
            e.Title,
            e.ShortDescription,
            e.EventType,
            e.EventType.ToString(),
            e.ImpactScore.Value,
            e.Period.Start,
            e.Period.End,
            e.Period.IsOngoing,
            companyName,
            companyLogoUrl,
            e.Achievements.OrderBy(a => a.DisplayOrder).Select(a => new AchievementDto(
                a.Id, a.Title, a.Description,
                a.MetricValue, a.MetricUnit,
                a.EvidenceUrl, a.EvidenceType?.ToString())).ToList(),
            e.Skills.Select(s => skillMap.TryGetValue(s.SkillId, out var name) ? name : "Unknown").ToList());
}
