namespace JobPortal.Application.Domain.Models;

public class ApplicationSkill
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string ProficiencyLevel { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public JobApplication? JobApplication { get; set; }
}
