namespace JobPortal.Catalog.Domain.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public string Website { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public CompanyContact? Contact { get; set; }
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
