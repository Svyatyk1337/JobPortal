using JobPortal.Application.Domain.Models;

namespace JobPortal.Application.Dal.Interfaces;

public interface IInterviewRepository
{
    Task<Interview?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Interview>> GetByApplicationIdAsync(int applicationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Interview>> GetUpcomingInterviewsAsync(CancellationToken cancellationToken = default);
    Task<int> CreateAsync(Interview interview, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Interview interview, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
