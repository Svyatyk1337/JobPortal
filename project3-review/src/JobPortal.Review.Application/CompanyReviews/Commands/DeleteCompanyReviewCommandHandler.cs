using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace JobPortal.Review.Application.CompanyReviews.Commands;

public class DeleteCompanyReviewCommandHandler : IRequestHandler<DeleteCompanyReviewCommand, bool>
{
    private readonly IRepository<CompanyReview> _repository;
    private readonly ILogger<DeleteCompanyReviewCommandHandler> _logger;

    public DeleteCompanyReviewCommandHandler(
        IRepository<CompanyReview> repository,
        ILogger<DeleteCompanyReviewCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteCompanyReviewCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting company review with ID {ReviewId}", request.Id);

        var review = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (review is null)
        {
            _logger.LogWarning("Company review with ID {ReviewId} not found", request.Id);
            return false;
        }

        await _repository.DeleteAsync(request.Id, cancellationToken);

        _logger.LogInformation("Company review with ID {ReviewId} deleted successfully", request.Id);

        return true;
    }
}
