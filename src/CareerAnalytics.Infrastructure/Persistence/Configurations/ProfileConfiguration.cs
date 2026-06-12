using CareerAnalytics.Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerAnalytics.Infrastructure.Persistence.Configurations;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("profiles");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.UserId).HasColumnName("user_id");
        builder.Property(p => p.Headline).HasColumnName("headline").HasMaxLength(200);
        builder.Property(p => p.Summary).HasColumnName("summary").HasMaxLength(2000);
        builder.Property(p => p.Location).HasColumnName("location").HasMaxLength(100);
        builder.Property(p => p.AvatarUrl).HasColumnName("avatar_url").HasMaxLength(500);
        builder.Property(p => p.YearsOfExperience).HasColumnName("years_of_experience");
        builder.Property(p => p.CreatedAt).HasColumnName("created_at");
        builder.Property(p => p.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(p => p.UserId).IsUnique();

        builder.OwnsOne(p => p.SocialLinks, links =>
        {
            links.Property(l => l.LinkedInUrl).HasColumnName("linkedin_url").HasMaxLength(500);
            links.Property(l => l.GitHubUrl).HasColumnName("github_url").HasMaxLength(500);
            links.Property(l => l.PortfolioUrl).HasColumnName("portfolio_url").HasMaxLength(500);
        });

        builder.OwnsOne(p => p.Theme, theme =>
        {
            theme.Property(t => t.Value).HasColumnName("profile_theme").HasMaxLength(20).IsRequired();
        });
    }
}
