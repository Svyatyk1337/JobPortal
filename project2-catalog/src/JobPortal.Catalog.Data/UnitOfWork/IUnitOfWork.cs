using JobPortal.Catalog.Data.Repositories;

namespace JobPortal.Catalog.Data.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository Companies { get; }
    IJobRepository Jobs { get; }
    ISkillTagRepository SkillTags { get; }
    IRepository<Domain.Entities.CompanyContact> CompanyContacts { get; }
    IRepository<Domain.Entities.JobCategory> JobCategories { get; }
    IRepository<Domain.Entities.JobSkillRequirement> JobSkillRequirements { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
