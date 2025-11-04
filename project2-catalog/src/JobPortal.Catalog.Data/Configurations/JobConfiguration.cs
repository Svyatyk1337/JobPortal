using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobPortal.Catalog.Data.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("jobs");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(j => j.Description)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(j => j.SalaryMin)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(j => j.SalaryMax)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(j => j.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(j => j.EmploymentType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(j => j.IsActive)
            .IsRequired();

        builder.Property(j => j.PostedAt)
            .IsRequired();

        // Relationships
        builder.HasOne(j => j.Category)
            .WithMany(jc => jc.Jobs)
            .HasForeignKey(j => j.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(j => j.CompanyId);
        builder.HasIndex(j => j.CategoryId);
        builder.HasIndex(j => j.IsActive);
        builder.HasIndex(j => j.PostedAt);

        // Check constraint for salary
        builder.ToTable(t => t.HasCheckConstraint("CK_Jobs_Salary", "salary_max >= salary_min"));
    }
}
