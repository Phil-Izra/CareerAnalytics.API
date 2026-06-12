using CareerAnalytics.Domain.Analytics;
using CareerAnalytics.Domain.CareerEvents;
using CareerAnalytics.Domain.CareerEvents.Entities;
using CareerAnalytics.Domain.Common;
using CareerAnalytics.Domain.Companies;
using CareerAnalytics.Domain.Profiles;
using CareerAnalytics.Domain.Skills;
using CareerAnalytics.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CareerAnalytics.Infrastructure.Persistence;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IPublisher publisher) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CareerEvent> CareerEvents => Set<CareerEvent>();
    public DbSet<EventAchievement> EventAchievements => Set<EventAchievement>();
    public DbSet<EventSkill> EventSkills => Set<EventSkill>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<UserSkill> UserSkills => Set<UserSkill>();
    public DbSet<CareerMetricsSnapshot> CareerMetricsSnapshots => Set<CareerMetricsSnapshot>();
    public DbSet<ProfileView> ProfileViews => Set<ProfileView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var aggregates = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var events = aggregates.SelectMany(a => a.DomainEvents).ToList();
        aggregates.ForEach(a => a.ClearDomainEvents());

        foreach (var domainEvent in events)
            await publisher.Publish(domainEvent, cancellationToken);
    }
}
