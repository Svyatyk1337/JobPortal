using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobPortal.Catalog.Data.Configurations;

public class CompanyContactConfiguration : IEntityTypeConfiguration<CompanyContact>
{
    public void Configure(EntityTypeBuilder<CompanyContact> builder)
    {
        builder.ToTable("company_contacts");

        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(cc => cc.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(cc => cc.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(cc => cc.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cc => cc.Country)
            .IsRequired()
            .HasMaxLength(100);

        // Index on CompanyId (unique because 1:1)
        builder.HasIndex(cc => cc.CompanyId)
            .IsUnique();
    }
}
