namespace JobPortal.Application.Domain.Models;

public class JobApplication
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime SubmittedDate { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Candidate? Candidate { get; set; }
    public ApplicationDetails? ApplicationDetails { get; set; }
    public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    public ICollection<ApplicationSkill> ApplicationSkills { get; set; } = new List<ApplicationSkill>();
}
