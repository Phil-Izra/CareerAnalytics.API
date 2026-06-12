using CareerAnalytics.Application.CareerEvents.DTOs;
using CareerAnalytics.Application.Common.Models;
using CareerAnalytics.Domain.Analytics.Repositories;
using CareerAnalytics.Domain.Analytics.ValueObjects;
using CareerAnalytics.Domain.CareerEvents.Enums;
using CareerAnalytics.Domain.CareerEvents.Repositories;
using CareerAnalytics.Domain.Companies.Repositories;
using CareerAnalytics.Domain.Profiles.Repositories;
using CareerAnalytics.Domain.Skills.Repositories;
using CareerAnalytics.Domain.Users.Repositories;
using MediatR;

namespace CareerAnalytics.Application.CareerEvents.Queries.GetPublicDashboard;

public sealed class GetPublicDashboardQueryHandler(
    IUserRepository userRepository,
    IProfileRepository profileRepository,
    ICareerEventRepository careerEventRepository,
    ICompanyRepository companyRepository,
    ISkillRepository skillRepository,
    IAnalyticsRepository analyticsRepository) : IRequestHandler<GetPublicDashboardQuery, Result<PublicDashboardDto>>
{
    public async Task<Result<PublicDashboardDto>> Handle(
        GetPublicDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (user is null || !user.IsPublicProfile)
            return Result.Failure<PublicDashboardDto>("Profile.NotFound", "Public profile not found.");

        var profile = await profileRepository.GetByUserIdAsync(user.Id, cancellationToken);
        if (profile is null)
            return Result.Failure<PublicDashboardDto>("Profile.NotFound", "Profile not found.");

        var eventsWithSkills = await careerEventRepository.GetWithSkillsByUserIdAsync(user.Id, cancellationToken);
        var publicEvents = eventsWithSkills.Where(e => e.IsPublic).OrderBy(e => e.Period.Start).ToList();

        var allSkills = await skillRepository.GetAllAsync(cancellationToken);
        var skillMap = allSkills.ToDictionary(s => s.Id, s => s);

        // Build timeline DTOs
        var timelineDtos = new List<CareerEventDto>();
        foreach (var e in publicEvents)
        {
            string? companyName = null;
            string? companyLogoUrl = null;
            if (e.CompanyId.HasValue)
            {
                var company = await companyRepository.GetByIdAsync(e.CompanyId.Value, cancellationToken);
                companyName = company?.Name;
                companyLogoUrl = company?.LogoUrl;
            }

            timelineDtos.Add(new CareerEventDto(
                e.Id, e.Title, e.ShortDescription,
                e.EventType, e.EventType.ToString(),
                e.ImpactScore.Value,
                e.Period.Start, e.Period.End, e.Period.IsOngoing,
                companyName, companyLogoUrl,
                e.Achievements.OrderBy(a => a.DisplayOrder)
                    .Select(a => new AchievementDto(
                        a.Id, a.Title, a.Description,
                        a.MetricValue, a.MetricUnit,
                        a.EvidenceUrl, a.EvidenceType?.ToString())).ToList(),
                e.Skills.Select(s => skillMap.TryGetValue(s.SkillId, out var sk) ? sk.Name : "Unknown").ToList()));
        }

        // Career metrics panel
        var promotionCount = publicEvents.Count(e => e.EventType == EventType.Promotion);
        var certCount = publicEvents.Count(e => e.EventType == EventType.Certification);
        var projectCount = publicEvents.Count(e => e.EventType == EventType.Project);
        var leadershipCount = publicEvents.Count(e => e.EventType == EventType.Leadership);

        var companiesWithIndustry = new List<string>();
        foreach (var e in publicEvents.Where(e => e.CompanyId.HasValue))
        {
            var company = await companyRepository.GetByIdAsync(e.CompanyId!.Value, cancellationToken);
            if (company?.Industry is not null)
                companiesWithIndustry.Add(company.Industry);
        }
        var industries = companiesWithIndustry.Distinct().ToList();

        // Score timeline for career graph
        var snapshots = await analyticsRepository.GetSnapshotsByUserIdAsync(user.Id, PeriodType.Yearly, cancellationToken);
        var scoreTimeline = snapshots
            .OrderBy(s => s.PeriodDate)
            .Select(s => new ScoreDataPoint(s.PeriodDate, s.TotalScore, s.PeriodType))
            .ToList();

        var totalScore = snapshots.Any() ? snapshots.Max(s => s.TotalScore) : publicEvents.Sum(e => e.ImpactScore.Value);

        // Skill evolution: group EventSkills by skill, ordered by event start date
        var skillEvolution = publicEvents
            .SelectMany(e => e.Skills.Select(s => (Event: e, SkillId: s.SkillId)))
            .GroupBy(x => x.SkillId)
            .Where(g => skillMap.ContainsKey(g.Key))
            .Select(g =>
            {
                var skill = skillMap[g.Key];
                var periods = g
                    .OrderBy(x => x.Event.Period.Start)
                    .Select(x => new SkillPeriodSnapshot(x.Event.Period.Start, x.Event.Title, x.Event.ImpactScore.Value))
                    .ToList();
                return new SkillEvolutionEntry(skill.Name, skill.Category.ToString(), periods);
            })
            .OrderBy(s => s.SkillName)
            .ToList();

        var viewCount = await analyticsRepository.GetProfileViewCountAsync(user.Id, cancellationToken);

        return Result.Success(new PublicDashboardDto(
            user.FullName,
            user.Slug.Value,
            profile.Headline,
            profile.Summary,
            profile.Location,
            profile.AvatarUrl,
            profile.SocialLinks.LinkedInUrl,
            profile.SocialLinks.GitHubUrl,
            profile.SocialLinks.PortfolioUrl,
            profile.Theme.Value,
            profile.YearsOfExperience,
            totalScore,
            promotionCount,
            certCount,
            projectCount,
            leadershipCount,
            industries,
            scoreTimeline,
            timelineDtos,
            skillEvolution,
            viewCount));
    }
}
