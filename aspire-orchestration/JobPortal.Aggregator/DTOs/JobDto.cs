namespace JobPortal.Aggregator.DTOs;

public class JobDto
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
    public bool IsActive { get; set; }
    public DateTime PostedAt { get; set; }
}
