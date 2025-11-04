using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Catalog.Data.Repositories;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Eager Loading - 1:1 relationship
    public async Task<Company?> GetWithContactAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Contact)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    // Eager Loading - 1:Many relationship
    public async Task<Company?> GetWithJobsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Jobs)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Company>> GetByIndustryAsync(string industry, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.Industry == industry)
            .ToListAsync(cancellationToken);
    }
}
