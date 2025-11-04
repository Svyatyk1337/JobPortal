using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobPortal.Catalog.Data.Configurations;

public class JobCategoryConfiguration : IEntityTypeConfiguration<JobCategory>
{
    public void Configure(EntityTypeBuilder<JobCategory> builder)
    {
        builder.ToTable("job_categories");

        builder.HasKey(jc => jc.Id);

        builder.Property(jc => jc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(jc => jc.Description)
            .HasMaxLength(500);

        // Index
        builder.HasIndex(jc => jc.Name)
            .IsUnique();
    }
}
