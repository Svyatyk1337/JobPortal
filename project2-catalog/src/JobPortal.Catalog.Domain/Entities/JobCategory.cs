namespace JobPortal.Catalog.Domain.Entities;

public class JobCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
