namespace JobPortal.Catalog.Domain.Entities;

public class Job
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal SalaryMin { get; set; }
    public decimal SalaryMax { get; set; }
    public string Location { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime PostedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Company Company { get; set; } = null!;
    public JobCategory Category { get; set; } = null!;
    public ICollection<JobSkillRequirement> SkillRequirements { get; set; } = new List<JobSkillRequirement>();
}
