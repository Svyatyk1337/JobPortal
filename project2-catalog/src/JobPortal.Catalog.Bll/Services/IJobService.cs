using JobPortal.Catalog.Bll.DTOs;

namespace JobPortal.Catalog.Bll.Services;

public interface IJobService
{
    Task<IEnumerable<JobDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<JobDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<JobWithDetailsDto?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<JobDto> CreateAsync(CreateJobDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateJobDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<JobDto>> GetActiveJobsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<JobDto>> GetJobsByCompanyAsync(int companyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<JobDto>> GetJobsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
}
