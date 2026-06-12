using CareerAnalytics.Domain.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerAnalytics.Infrastructure.Persistence.Configurations;

public sealed class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("skills");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        builder.Property(s => s.Category).HasColumnName("category").HasConversion<string>();
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(s => s.Name).IsUnique();
    }
}

public sealed class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.ToTable("user_skills");

        builder.HasKey(us => us.Id);
        builder.Property(us => us.Id).HasColumnName("id");
        builder.Property(us => us.UserId).HasColumnName("user_id");
        builder.Property(us => us.SkillId).HasColumnName("skill_id");
        builder.Property(us => us.FirstUsedDate).HasColumnName("first_used_date");
        builder.Property(us => us.LastUsedDate).HasColumnName("last_used_date");
        builder.Property(us => us.CreatedAt).HasColumnName("created_at");
        builder.Property(us => us.UpdatedAt).HasColumnName("updated_at");

        builder.OwnsOne(us => us.ProficiencyLevel, pl =>
        {
            pl.Property(p => p.Value).HasColumnName("proficiency_level");
        });

        builder.HasIndex(us => new { us.UserId, us.SkillId }).IsUnique();
    }
}
