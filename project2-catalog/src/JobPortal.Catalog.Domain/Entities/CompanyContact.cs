namespace JobPortal.Catalog.Domain.Entities;

public class CompanyContact
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // Navigation property
    public Company Company { get; set; } = null!;
}
