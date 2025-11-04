namespace JobPortal.Catalog.Domain.Entities;

public class SkillTag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<JobSkillRequirement> JobSkillRequirements { get; set; } = new List<JobSkillRequirement>();
}
