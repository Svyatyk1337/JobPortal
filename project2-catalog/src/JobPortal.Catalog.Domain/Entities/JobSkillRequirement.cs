namespace JobPortal.Catalog.Domain.Entities;

public class JobSkillRequirement
{
    public int JobId { get; set; }
    public int SkillTagId { get; set; }
    public string RequiredLevel { get; set; } = string.Empty;
    public bool IsRequired { get; set; }

    // Navigation properties
    public Job Job { get; set; } = null!;
    public SkillTag SkillTag { get; set; } = null!;
}
