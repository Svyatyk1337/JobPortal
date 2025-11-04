namespace JobPortal.Catalog.Bll.DTOs;

public class SkillTagDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public class CreateSkillTagDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public class SkillRequirementDto
{
    public int SkillTagId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string RequiredLevel { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
}
