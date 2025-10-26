namespace JobPortal.Application.Dal.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICandidateRepository Candidates { get; }
    IJobApplicationRepository JobApplications { get; }
    IInterviewRepository Interviews { get; }

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
