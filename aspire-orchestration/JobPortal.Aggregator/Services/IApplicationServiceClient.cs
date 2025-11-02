using JobPortal.Aggregator.DTOs;

namespace JobPortal.Aggregator.Services;

public interface IApplicationServiceClient
{
    Task<CandidateDto?> GetCandidateAsync(int id, CancellationToken cancellationToken = default);
    Task<List<CandidateDto>> GetAllCandidatesAsync(CancellationToken cancellationToken = default);
    Task<List<JobApplicationDto>> GetApplicationsByCandidateAsync(int candidateId, CancellationToken cancellationToken = default);
    Task<JobApplicationDto?> GetApplicationAsync(int id, CancellationToken cancellationToken = default);
    Task<List<JobApplicationDto>> GetAllApplicationsAsync(CancellationToken cancellationToken = default);
    Task<List<InterviewDto>> GetInterviewsByApplicationAsync(int applicationId, CancellationToken cancellationToken = default);
    Task<List<InterviewDto>> GetAllInterviewsAsync(CancellationToken cancellationToken = default);
}
