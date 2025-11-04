using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobPortal.Catalog.Data.Configurations;

public class SkillTagConfiguration : IEntityTypeConfiguration<SkillTag>
{
    public void Configure(EntityTypeBuilder<SkillTag> builder)
    {
        builder.ToTable("skill_tags");

        builder.HasKey(st => st.Id);

        builder.Property(st => st.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(st => st.Category)
            .IsRequired()
            .HasMaxLength(100);

        // Index
        builder.HasIndex(st => st.Name)
            .IsUnique();
    }
}
