using CareerAnalytics.Domain.CareerEvents;
using CareerAnalytics.Domain.CareerEvents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerAnalytics.Infrastructure.Persistence.Configurations;

public sealed class CareerEventConfiguration : IEntityTypeConfiguration<CareerEvent>
{
    public void Configure(EntityTypeBuilder<CareerEvent> builder)
    {
        builder.ToTable("career_events");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.UserId).HasColumnName("user_id");
        builder.Property(e => e.CompanyId).HasColumnName("company_id");
        builder.Property(e => e.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
        builder.Property(e => e.ShortDescription).HasColumnName("short_description").HasMaxLength(500);
        builder.Property(e => e.EventType).HasColumnName("event_type").HasConversion<string>();
        builder.Property(e => e.IsPublic).HasColumnName("is_public");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

        builder.OwnsOne(e => e.ImpactScore, score =>
        {
            score.Property(s => s.Value).HasColumnName("impact_score");
        });

        builder.OwnsOne(e => e.Period, period =>
        {
            period.Property(p => p.Start).HasColumnName("event_start_date");
            period.Property(p => p.End).HasColumnName("event_end_date");
        });

        builder.HasMany(e => e.Achievements)
            .WithOne()
            .HasForeignKey(a => a.CareerEventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Skills)
            .WithOne()
            .HasForeignKey(s => s.CareerEventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(e => e.Achievements).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(e => e.Skills).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => new { e.UserId, e.IsPublic });
    }
}

public sealed class EventAchievementConfiguration : IEntityTypeConfiguration<EventAchievement>
{
    public void Configure(EntityTypeBuilder<EventAchievement> builder)
    {
        builder.ToTable("event_achievements");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.CareerEventId).HasColumnName("career_event_id");
        builder.Property(a => a.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
        builder.Property(a => a.Description).HasColumnName("description").HasMaxLength(1000);
        builder.Property(a => a.MetricValue).HasColumnName("metric_value").HasPrecision(10, 2);
        builder.Property(a => a.MetricUnit).HasColumnName("metric_unit").HasMaxLength(50);
        builder.Property(a => a.EvidenceUrl).HasColumnName("evidence_url").HasMaxLength(500);
        builder.Property(a => a.EvidenceType).HasColumnName("evidence_type").HasConversion<string>();
        builder.Property(a => a.DisplayOrder).HasColumnName("display_order");
        builder.Property(a => a.CreatedAt).HasColumnName("created_at");
        builder.Property(a => a.UpdatedAt).HasColumnName("updated_at");
    }
}

public sealed class EventSkillConfiguration : IEntityTypeConfiguration<EventSkill>
{
    public void Configure(EntityTypeBuilder<EventSkill> builder)
    {
        builder.ToTable("event_skills");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.CareerEventId).HasColumnName("career_event_id");
        builder.Property(s => s.SkillId).HasColumnName("skill_id");
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(s => new { s.CareerEventId, s.SkillId }).IsUnique();
    }
}
