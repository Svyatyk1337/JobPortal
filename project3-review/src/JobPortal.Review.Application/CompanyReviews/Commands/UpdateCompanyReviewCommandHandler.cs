using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobPortal.Review.Application.CompanyReviews.Commands;

public class UpdateCompanyReviewCommandHandler : IRequestHandler<UpdateCompanyReviewCommand, bool>
{
    private readonly IRepository<CompanyReview> _repository;
    private readonly ILogger<UpdateCompanyReviewCommandHandler> _logger;

    public UpdateCompanyReviewCommandHandler(
        IRepository<CompanyReview> repository,
        ILogger<UpdateCompanyReviewCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateCompanyReviewCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating company review with ID {ReviewId}", request.Id);

        var review = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (review is null)
        {
            _logger.LogWarning("Company review with ID {ReviewId} not found", request.Id);
            return false;
        }

        review.Update(
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

        await _repository.UpdateAsync(request.Id, review, cancellationToken);

        _logger.LogInformation("Company review with ID {ReviewId} updated successfully", request.Id);

        return true;
    }
}
