using CareerAnalytics.Application.Common.Interfaces;
using CareerAnalytics.Domain.Analytics.Repositories;
using CareerAnalytics.Domain.Analytics.Services;
using CareerAnalytics.Domain.CareerEvents.Repositories;
using CareerAnalytics.Domain.Companies.Repositories;
using CareerAnalytics.Domain.Profiles.Repositories;
using CareerAnalytics.Domain.Skills.Repositories;
using CareerAnalytics.Domain.Users.Repositories;
using CareerAnalytics.Infrastructure.Persistence;
using CareerAnalytics.Infrastructure.Persistence.Repositories;
using CareerAnalytics.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CareerAnalytics.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICareerEventRepository, CareerEventRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
