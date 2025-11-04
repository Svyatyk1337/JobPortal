using JobPortal.Review.Domain.Common;
using JobPortal.Review.Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace JobPortal.Review.Domain.Entities;

public class CompanyReview : BaseEntity
{
    [BsonElement("company_id")]
    public int CompanyId { get; private set; }

    [BsonElement("user_id")]
    public int UserId { get; private set; }

    [BsonElement("overall_rating")]
    public Rating OverallRating { get; private set; } = null!;

    [BsonElement("work_life_balance_rating")]
    public Rating WorkLifeBalanceRating { get; private set; } = null!;

    [BsonElement("culture_rating")]
    public Rating CultureRating { get; private set; } = null!;

    [BsonElement("management_rating")]
    public Rating ManagementRating { get; private set; } = null!;

    [BsonElement("compensation_rating")]
    public Rating CompensationRating { get; private set; } = null!;

    [BsonElement("title")]
    public string Title { get; private set; } = string.Empty;

    [BsonElement("review_text")]
    public string ReviewText { get; private set; } = string.Empty;

    [BsonElement("pros")]
    public string Pros { get; private set; } = string.Empty;

    [BsonElement("cons")]
    public string Cons { get; private set; } = string.Empty;

    [BsonElement("is_current_employee")]
    public bool IsCurrentEmployee { get; private set; }

    [BsonElement("job_title")]
    public string JobTitle { get; private set; } = string.Empty;

    private CompanyReview() { } // For MongoDB

    public static CompanyReview Create(
        int companyId,
        int userId,
        double overallRating,
        double workLifeBalanceRating,
        double cultureRating,
        double managementRating,
        double compensationRating,
        string title,
        string reviewText,
        string pros,
        string cons,
        bool isCurrentEmployee,
        string jobTitle)
    {
        return new CompanyReview
        {
            CompanyId = companyId,
            UserId = userId,
            OverallRating = Rating.Create(overallRating),
            WorkLifeBalanceRating = Rating.Create(workLifeBalanceRating),
            CultureRating = Rating.Create(cultureRating),
            ManagementRating = Rating.Create(managementRating),
            CompensationRating = Rating.Create(compensationRating),
            Title = title,
            ReviewText = reviewText,
            Pros = pros,
            Cons = cons,
            IsCurrentEmployee = isCurrentEmployee,
            JobTitle = jobTitle
        };
    }

    public void Update(
        double? overallRating = null,
        double? workLifeBalanceRating = null,
        double? cultureRating = null,
        double? managementRating = null,
        double? compensationRating = null,
        string? title = null,
        string? reviewText = null,
        string? pros = null,
        string? cons = null,
        bool? isCurrentEmployee = null,
        string? jobTitle = null)
    {
        if (overallRating.HasValue) OverallRating = Rating.Create(overallRating.Value);
        if (workLifeBalanceRating.HasValue) WorkLifeBalanceRating = Rating.Create(workLifeBalanceRating.Value);
        if (cultureRating.HasValue) CultureRating = Rating.Create(cultureRating.Value);
        if (managementRating.HasValue) ManagementRating = Rating.Create(managementRating.Value);
        if (compensationRating.HasValue) CompensationRating = Rating.Create(compensationRating.Value);
        if (title != null) Title = title;
        if (reviewText != null) ReviewText = reviewText;
        if (pros != null) Pros = pros;
        if (cons != null) Cons = cons;
        if (isCurrentEmployee.HasValue) IsCurrentEmployee = isCurrentEmployee.Value;
        if (jobTitle != null) JobTitle = jobTitle;

        MarkAsUpdated();
    }
}
