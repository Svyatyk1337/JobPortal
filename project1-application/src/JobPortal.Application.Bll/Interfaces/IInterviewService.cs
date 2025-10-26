using JobPortal.Application.Bll.DTOs;

namespace JobPortal.Application.Bll.Interfaces;

public interface IInterviewService
{
    Task<InterviewDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<InterviewDto>> GetByApplicationIdAsync(int applicationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<InterviewDto>> GetUpcomingInterviewsAsync(CancellationToken cancellationToken = default);
    Task<InterviewDto> CreateAsync(CreateInterviewDto createDto, CancellationToken cancellationToken = default);
    Task<InterviewDto> UpdateAsync(UpdateInterviewDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
