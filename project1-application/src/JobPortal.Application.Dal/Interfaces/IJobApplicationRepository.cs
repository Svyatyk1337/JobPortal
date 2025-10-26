using JobPortal.Application.Domain.Models;

namespace JobPortal.Application.Dal.Interfaces;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<JobApplication?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<JobApplication>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<JobApplication>> GetByCandidateIdAsync(int candidateId, CancellationToken cancellationToken = default);
    Task<IEnumerable<JobApplication>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(JobApplication application, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(JobApplication application, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
