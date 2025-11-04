namespace JobPortal.Catalog.Bll.DTOs;

public class JobCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateJobCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
