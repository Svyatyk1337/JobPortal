using JobPortal.Application.Bll.DTOs;

namespace JobPortal.Application.Bll.Interfaces;

public interface IJobApplicationService
{
    Task<JobApplicationDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<JobApplicationDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<JobApplicationDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<JobApplicationDto>> GetByCandidateIdAsync(int candidateId, CancellationToken cancellationToken = default);
    Task<IEnumerable<JobApplicationDto>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<JobApplicationDto> CreateWithDetailsAsync(CreateJobApplicationDto createDto, CancellationToken cancellationToken = default);
    Task<JobApplicationDto> UpdateAsync(UpdateJobApplicationDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
