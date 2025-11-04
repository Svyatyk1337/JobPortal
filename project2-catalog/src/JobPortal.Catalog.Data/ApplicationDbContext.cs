using JobPortal.Catalog.Data.Configurations;
using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Catalog.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyContact> CompanyContacts => Set<CompanyContact>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobCategory> JobCategories => Set<JobCategory>();
    public DbSet<SkillTag> SkillTags => Set<SkillTag>();
    public DbSet<JobSkillRequirement> JobSkillRequirements => Set<JobSkillRequirement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyContactConfiguration());
        modelBuilder.ApplyConfiguration(new JobConfiguration());
        modelBuilder.ApplyConfiguration(new JobCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SkillTagConfiguration());
        modelBuilder.ApplyConfiguration(new JobSkillRequirementConfiguration());

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Job Categories
        modelBuilder.Entity<JobCategory>().HasData(
            new JobCategory { Id = 1, Name = "Software Development", Description = "Software engineering and development roles" },
            new JobCategory { Id = 2, Name = "Data Science", Description = "Data analysis and machine learning roles" },
            new JobCategory { Id = 3, Name = "DevOps", Description = "DevOps and infrastructure roles" },
            new JobCategory { Id = 4, Name = "Design", Description = "UI/UX and graphic design roles" }
        );

        // Seed Skill Tags
        modelBuilder.Entity<SkillTag>().HasData(
            new SkillTag { Id = 1, Name = "C#", Category = "Programming Language" },
            new SkillTag { Id = 2, Name = ".NET", Category = "Framework" },
            new SkillTag { Id = 3, Name = "Python", Category = "Programming Language" },
            new SkillTag { Id = 4, Name = "JavaScript", Category = "Programming Language" },
            new SkillTag { Id = 5, Name = "React", Category = "Framework" },
            new SkillTag { Id = 6, Name = "SQL", Category = "Database" },
            new SkillTag { Id = 7, Name = "Docker", Category = "DevOps" },
            new SkillTag { Id = 8, Name = "Kubernetes", Category = "DevOps" }
        );

        // Seed Companies
        modelBuilder.Entity<Company>().HasData(
            new Company
            {
                Id = 1,
                Name = "TechCorp Solutions",
                Description = "Leading software development company",
                Industry = "Information Technology",
                EmployeeCount = 500,
                Website = "https://techcorp.example.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-12)
            },
            new Company
            {
                Id = 2,
                Name = "DataMinds Inc",
                Description = "Data science and analytics consulting",
                Industry = "Consulting",
                EmployeeCount = 150,
                Website = "https://dataminds.example.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-8)
            },
            new Company
            {
                Id = 3,
                Name = "CloudNative Systems",
                Description = "Cloud infrastructure and DevOps services",
                Industry = "Cloud Services",
                EmployeeCount = 300,
                Website = "https://cloudnative.example.com",
                CreatedAt = DateTime.UtcNow.AddMonths(-6)
            }
        );

        // Seed Company Contacts
        modelBuilder.Entity<CompanyContact>().HasData(
            new CompanyContact
            {
                Id = 1,
                CompanyId = 1,
                Email = "hr@techcorp.example.com",
                Phone = "+380441234567",
                Address = "123 Tech Street",
                City = "Kyiv",
                Country = "Ukraine"
            },
            new CompanyContact
            {
                Id = 2,
                CompanyId = 2,
                Email = "careers@dataminds.example.com",
                Phone = "+380442345678",
                Address = "456 Data Avenue",
                City = "Lviv",
                Country = "Ukraine"
            },
            new CompanyContact
            {
                Id = 3,
                CompanyId = 3,
                Email = "jobs@cloudnative.example.com",
                Phone = "+380443456789",
                Address = "789 Cloud Boulevard",
                City = "Kharkiv",
                Country = "Ukraine"
            }
        );

        // Seed Jobs
        modelBuilder.Entity<Job>().HasData(
            new Job
            {
                Id = 1,
                CompanyId = 1,
                Title = "Senior .NET Developer",
                Description = "We are looking for an experienced .NET developer to join our team.",
                CategoryId = 1,
                SalaryMin = 3000m,
                SalaryMax = 5000m,
                Location = "Kyiv, Ukraine",
                EmploymentType = "Full-time",
                ExperienceYears = 5,
                IsActive = true,
                PostedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Job
            {
                Id = 2,
                CompanyId = 2,
                Title = "Data Scientist",
                Description = "Join our data science team to build ML models.",
                CategoryId = 2,
                SalaryMin = 2500m,
                SalaryMax = 4500m,
                Location = "Lviv, Ukraine",
                EmploymentType = "Full-time",
                ExperienceYears = 3,
                IsActive = true,
                PostedAt = DateTime.UtcNow.AddDays(-7)
            },
            new Job
            {
                Id = 3,
                CompanyId = 3,
                Title = "DevOps Engineer",
                Description = "Looking for a DevOps engineer with Kubernetes experience.",
                CategoryId = 3,
                SalaryMin = 2800m,
                SalaryMax = 4800m,
                Location = "Kharkiv, Ukraine",
                EmploymentType = "Full-time",
                ExperienceYears = 4,
                IsActive = true,
                PostedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Job
            {
                Id = 4,
                CompanyId = 1,
                Title = "React Developer",
                Description = "Frontend developer position with React expertise.",
                CategoryId = 1,
                SalaryMin = 2000m,
                SalaryMax = 3500m,
                Location = "Remote",
                EmploymentType = "Full-time",
                ExperienceYears = 2,
                IsActive = true,
                PostedAt = DateTime.UtcNow.AddDays(-3)
            }
        );

        // Seed Job Skill Requirements
        modelBuilder.Entity<JobSkillRequirement>().HasData(
            new JobSkillRequirement { JobId = 1, SkillTagId = 1, RequiredLevel = "Expert", IsRequired = true },
            new JobSkillRequirement { JobId = 1, SkillTagId = 2, RequiredLevel = "Expert", IsRequired = true },
            new JobSkillRequirement { JobId = 1, SkillTagId = 6, RequiredLevel = "Advanced", IsRequired = true },
            new JobSkillRequirement { JobId = 2, SkillTagId = 3, RequiredLevel = "Expert", IsRequired = true },
            new JobSkillRequirement { JobId = 2, SkillTagId = 6, RequiredLevel = "Advanced", IsRequired = true },
            new JobSkillRequirement { JobId = 3, SkillTagId = 7, RequiredLevel = "Expert", IsRequired = true },
            new JobSkillRequirement { JobId = 3, SkillTagId = 8, RequiredLevel = "Advanced", IsRequired = true },
            new JobSkillRequirement { JobId = 4, SkillTagId = 4, RequiredLevel = "Expert", IsRequired = true },
            new JobSkillRequirement { JobId = 4, SkillTagId = 5, RequiredLevel = "Expert", IsRequired = true }
        );
    }
}
