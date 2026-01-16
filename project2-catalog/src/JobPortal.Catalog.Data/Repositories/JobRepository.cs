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

    public async Task<(IEnumerable<Job> Items, int TotalCount)> GetJobsPagedAsync(
        int page,
        int pageSize,
        System.Linq.Expressions.Expression<Func<Job, bool>>? filter = null,
        string? sortBy = null,
        bool sortDescending = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Job> query = _dbSet
            .Include(j => j.Company)
            .Include(j => j.Category);

        // Apply filter
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "title" => sortDescending ? query.OrderByDescending(j => j.Title) : query.OrderBy(j => j.Title),
            "salary" => sortDescending ? query.OrderByDescending(j => j.SalaryMax) : query.OrderBy(j => j.SalaryMax),
            "postedat" => sortDescending ? query.OrderByDescending(j => j.PostedAt) : query.OrderBy(j => j.PostedAt),
            "company" => sortDescending ? query.OrderByDescending(j => j.Company.Name) : query.OrderBy(j => j.Company.Name),
            _ => query.OrderByDescending(j => j.PostedAt) // Default sort
        };

        // Apply pagination
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
