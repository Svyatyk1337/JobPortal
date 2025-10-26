namespace JobPortal.Application.Domain.Models;

public class ApplicationDetails
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string? ResumeUrl { get; set; }
    public string? CoverLetter { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property (1:1)
    public JobApplication? JobApplication { get; set; }
}
