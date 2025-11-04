using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobPortal.Catalog.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("companies");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.Industry)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Website)
            .HasMaxLength(255);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        // 1:1 relationship with CompanyContact
        builder.HasOne(c => c.Contact)
            .WithOne(cc => cc.Company)
            .HasForeignKey<CompanyContact>(cc => cc.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1:Many relationship with Jobs
        builder.HasMany(c => c.Jobs)
            .WithOne(j => j.Company)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index
        builder.HasIndex(c => c.Name);
    }
}
