namespace JobPortal.Catalog.Bll.DTOs;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public string Website { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateCompanyDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public string Website { get; set; } = string.Empty;
}

public class UpdateCompanyDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public string Website { get; set; } = string.Empty;
}

public class CompanyWithContactDto : CompanyDto
{
    public CompanyContactDto? Contact { get; set; }
}
