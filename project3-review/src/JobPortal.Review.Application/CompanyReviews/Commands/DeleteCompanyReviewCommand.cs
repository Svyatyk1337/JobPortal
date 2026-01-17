using MediatR;

namespace JobPortal.Review.Application.CompanyReviews.Commands;

public record DeleteCompanyReviewCommand(
    string Id
) : IRequest<bool>;
