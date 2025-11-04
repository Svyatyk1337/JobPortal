using JobPortal.Catalog.Domain.Entities;

namespace JobPortal.Catalog.Data.Repositories;

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetWithContactAsync(int id, CancellationToken cancellationToken = default);
    Task<Company?> GetWithJobsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Company>> GetByIndustryAsync(string industry, CancellationToken cancellationToken = default);
}
