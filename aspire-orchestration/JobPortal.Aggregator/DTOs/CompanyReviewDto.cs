namespace JobPortal.Aggregator.DTOs;

public class CompanyReviewDto
{
    public string Id { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public int UserId { get; set; }
    public double OverallRating { get; set; }
    public double WorkLifeBalanceRating { get; set; }
    public double CultureRating { get; set; }
    public double ManagementRating { get; set; }
    public double CompensationRating { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public string Pros { get; set; } = string.Empty;
    public string Cons { get; set; } = string.Empty;
    public bool IsCurrentEmployee { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
