using JobPortal.Catalog.Domain.Entities;

namespace JobPortal.Catalog.Data.Repositories;

public interface IJobRepository : IRepository<Job>
{
    Task<Job?> GetWithCompanyAndCategoryAsync(int id, CancellationToken cancellationToken = default);
    Task<Job?> GetWithSkillsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Job>> GetActiveJobsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Job>> GetJobsByCompanyAsync(int companyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Job>> GetJobsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
}
