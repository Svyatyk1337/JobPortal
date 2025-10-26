namespace JobPortal.Application.Domain.Models;

public class Candidate
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int YearsOfExperience { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}
