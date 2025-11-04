using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobPortal.Review.Application.CompanyReviews.Commands;

public class CreateCompanyReviewCommandHandler : IRequestHandler<CreateCompanyReviewCommand, string>
{
    private readonly IRepository<CompanyReview> _repository;
    private readonly ILogger<CreateCompanyReviewCommandHandler> _logger;

    public CreateCompanyReviewCommandHandler(
        IRepository<CompanyReview> repository,
        ILogger<CreateCompanyReviewCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<string> Handle(CreateCompanyReviewCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating company review for company {CompanyId}", request.CompanyId);

        var review = CompanyReview.Create(
            request.CompanyId,
            request.UserId,
            request.OverallRating,
            request.WorkLifeBalanceRating,
            request.CultureRating,
            request.ManagementRating,
            request.CompensationRating,
            request.Title,
            request.ReviewText,
            request.Pros,
            request.Cons,
            request.IsCurrentEmployee,
            request.JobTitle);

        var id = await _repository.AddAsync(review, cancellationToken);

        _logger.LogInformation("Company review created with ID {ReviewId}", id);

        return id;
    }
}
