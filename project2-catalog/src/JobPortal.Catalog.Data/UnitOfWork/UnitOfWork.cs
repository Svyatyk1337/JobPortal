using JobPortal.Catalog.Data.Repositories;
using JobPortal.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobPortal.Catalog.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    private ICompanyRepository? _companies;
    private IJobRepository? _jobs;
    private ISkillTagRepository? _skillTags;
    private IRepository<CompanyContact>? _companyContacts;
    private IRepository<JobCategory>? _jobCategories;
    private IRepository<JobSkillRequirement>? _jobSkillRequirements;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICompanyRepository Companies
    {
        get
        {
            _companies ??= new CompanyRepository(_context);
            return _companies;
        }
    }

    public IJobRepository Jobs
    {
        get
        {
            _jobs ??= new JobRepository(_context);
            return _jobs;
        }
    }

    public IRepository<CompanyContact> CompanyContacts
    {
        get
        {
            _companyContacts ??= new Repository<CompanyContact>(_context);
            return _companyContacts;
        }
    }

    public IRepository<JobCategory> JobCategories
    {
        get
        {
            _jobCategories ??= new Repository<JobCategory>(_context);
            return _jobCategories;
        }
    }

    public ISkillTagRepository SkillTags
    {
        get
        {
            _skillTags ??= new SkillTagRepository(_context);
            return _skillTags;
        }
    }

    public IRepository<JobSkillRequirement> JobSkillRequirements
    {
        get
        {
            _jobSkillRequirements ??= new Repository<JobSkillRequirement>(_context);
            return _jobSkillRequirements;
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
