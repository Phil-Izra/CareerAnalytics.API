using CareerAnalytics.Domain.Analytics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerAnalytics.Infrastructure.Persistence.Configurations;

public sealed class CareerMetricsSnapshotConfiguration : IEntityTypeConfiguration<CareerMetricsSnapshot>
{
    public void Configure(EntityTypeBuilder<CareerMetricsSnapshot> builder)
    {
        builder.ToTable("career_metrics_snapshots");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.UserId).HasColumnName("user_id");
        builder.Property(s => s.PeriodType).HasColumnName("period_type").HasConversion<string>();
        builder.Property(s => s.PeriodDate).HasColumnName("period_date");
        builder.Property(s => s.TotalScore).HasColumnName("total_score");
        builder.Property(s => s.TechnicalScore).HasColumnName("technical_score");
        builder.Property(s => s.LeadershipScore).HasColumnName("leadership_score");
        builder.Property(s => s.BusinessImpactScore).HasColumnName("business_impact_score");
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(s => new { s.UserId, s.PeriodType, s.PeriodDate }).IsUnique();
    }
}

public sealed class ProfileViewConfiguration : IEntityTypeConfiguration<ProfileView>
{
    public void Configure(EntityTypeBuilder<ProfileView> builder)
    {
        builder.ToTable("profile_views");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id");
        builder.Property(v => v.ProfileUserId).HasColumnName("profile_user_id");
        builder.Property(v => v.ViewerIpHash).HasColumnName("viewer_ip_hash").HasMaxLength(64).IsRequired();
        builder.Property(v => v.Country).HasColumnName("country").HasMaxLength(100);
        builder.Property(v => v.SessionId).HasColumnName("session_id").HasMaxLength(100);
        builder.Property(v => v.ViewedAt).HasColumnName("viewed_at");
        builder.Property(v => v.CreatedAt).HasColumnName("created_at");
        builder.Property(v => v.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(v => v.ProfileUserId);
        builder.HasIndex(v => v.ViewedAt);
    }
}
