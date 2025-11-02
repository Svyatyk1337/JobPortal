using JobPortal.Aggregator.DTOs;

namespace JobPortal.Aggregator.Services;

public interface IReviewServiceClient
{
    Task<List<CompanyReviewDto>> GetReviewsByCompanyAsync(int companyId, CancellationToken cancellationToken = default);
    Task<CompanyReviewDto?> GetReviewAsync(string id, CancellationToken cancellationToken = default);
}
