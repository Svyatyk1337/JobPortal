using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobPortal.Catalog.Data.Configurations;

public class JobSkillRequirementConfiguration : IEntityTypeConfiguration<JobSkillRequirement>
{
    public void Configure(EntityTypeBuilder<JobSkillRequirement> builder)
    {
        builder.ToTable("job_skill_requirements");

        // Composite primary key
        builder.HasKey(jsr => new { jsr.JobId, jsr.SkillTagId });

        builder.Property(jsr => jsr.RequiredLevel)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(jsr => jsr.IsRequired)
            .IsRequired();

        // Relationships
        builder.HasOne(jsr => jsr.Job)
            .WithMany(j => j.SkillRequirements)
            .HasForeignKey(jsr => jsr.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(jsr => jsr.SkillTag)
            .WithMany(st => st.JobSkillRequirements)
            .HasForeignKey(jsr => jsr.SkillTagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
