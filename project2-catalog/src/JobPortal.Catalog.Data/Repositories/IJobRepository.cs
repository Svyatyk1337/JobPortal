using JobPortal.Catalog.Domain.Entities;
using System.Linq.Expressions;

namespace JobPortal.Catalog.Data.Repositories;

public interface IJobRepository : IRepository<Job>
{
    Task<Job?> GetWithCompanyAndCategoryAsync(int id, CancellationToken cancellationToken = default);
    Task<Job?> GetWithSkillsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Job>> GetActiveJobsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Job>> GetJobsByCompanyAsync(int companyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Job>> GetJobsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Job> Items, int TotalCount)> GetJobsPagedAsync(
        int page,
        int pageSize,
        Expression<Func<Job, bool>>? filter = null,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default);
}
