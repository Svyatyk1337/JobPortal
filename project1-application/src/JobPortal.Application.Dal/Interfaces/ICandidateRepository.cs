using JobPortal.Application.Domain.Models;

namespace JobPortal.Application.Dal.Interfaces;

public interface ICandidateRepository
{
    Task<Candidate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Candidate>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Candidate?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Candidate candidate, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Candidate candidate, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
