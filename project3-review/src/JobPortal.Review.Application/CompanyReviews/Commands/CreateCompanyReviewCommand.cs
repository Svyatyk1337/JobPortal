using MediatR;

namespace JobPortal.Review.Application.CompanyReviews.Commands;

public record CreateCompanyReviewCommand(
    int CompanyId,
    int UserId,
    double OverallRating,
    double WorkLifeBalanceRating,
    double CultureRating,
    double ManagementRating,
    double CompensationRating,
    string Title,
    string ReviewText,
    string Pros,
    string Cons,
    bool IsCurrentEmployee,
    string JobTitle
) : IRequest<string>;
