using JobPortal.Aggregator.DTOs;

namespace JobPortal.Aggregator.Services;

public interface ICatalogServiceClient
{
    Task<JobDto?> GetJobAsync(int id, CancellationToken cancellationToken = default);
    Task<List<JobDto>> GetAllJobsAsync(CancellationToken cancellationToken = default);
    Task<CompanyDto?> GetCompanyAsync(int id, CancellationToken cancellationToken = default);
    Task<List<CompanyDto>> GetAllCompaniesAsync(CancellationToken cancellationToken = default);
    Task<List<JobDto>> GetJobsByCompanyAsync(int companyId, CancellationToken cancellationToken = default);
}
