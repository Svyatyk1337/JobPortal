using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Catalog.Data.Repositories;

public class JobRepository : Repository<Job>, IJobRepository
{
    public JobRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Eager Loading - Multiple relationships
    public async Task<Job?> GetWithCompanyAndCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(j => j.Company)
            .Include(j => j.Category)
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
    }

    // Eager Loading - Many-to-Many through junction table
    public async Task<Job?> GetWithSkillsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(j => j.SkillRequirements)
                .ThenInclude(sr => sr.SkillTag)
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Job>> GetActiveJobsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(j => j.IsActive)
            .Include(j => j.Company)
            .Include(j => j.Category)
            .OrderByDescending(j => j.PostedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Job>> GetJobsByCompanyAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(j => j.CompanyId == companyId)
            .Include(j => j.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Job>> GetJobsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(j => j.CategoryId == categoryId)
            .Include(j => j.Company)
            .ToListAsync(cancellationToken);
    }
}
