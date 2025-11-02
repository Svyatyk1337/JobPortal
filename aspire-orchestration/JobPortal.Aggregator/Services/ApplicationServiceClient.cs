using System.Net.Http.Json;
using JobPortal.Aggregator.DTOs;

namespace JobPortal.Aggregator.Services;

public class ApplicationServiceClient : IApplicationServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApplicationServiceClient> _logger;

    public ApplicationServiceClient(HttpClient httpClient, ILogger<ApplicationServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CandidateDto?> GetCandidateAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching candidate with ID {CandidateId}", id);
            return await _httpClient.GetFromJsonAsync<CandidateDto>($"/api/candidates/{id}", cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching candidate {CandidateId}", id);
            return null;
        }
    }

    public async Task<List<CandidateDto>> GetAllCandidatesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all candidates");
            var result = await _httpClient.GetFromJsonAsync<List<CandidateDto>>("/api/candidates", cancellationToken);
            return result ?? new List<CandidateDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching all candidates");
            return new List<CandidateDto>();
        }
    }

    public async Task<List<JobApplicationDto>> GetApplicationsByCandidateAsync(int candidateId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching applications for candidate {CandidateId}", candidateId);
            var result = await _httpClient.GetFromJsonAsync<List<JobApplicationDto>>($"/api/job-applications/candidate/{candidateId}", cancellationToken);
            return result ?? new List<JobApplicationDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching applications for candidate {CandidateId}", candidateId);
            return new List<JobApplicationDto>();
        }
    }

    public async Task<JobApplicationDto?> GetApplicationAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching application with ID {ApplicationId}", id);
            return await _httpClient.GetFromJsonAsync<JobApplicationDto>($"/api/job-applications/{id}", cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching application {ApplicationId}", id);
            return null;
        }
    }

    public async Task<List<JobApplicationDto>> GetAllApplicationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all applications");
            var result = await _httpClient.GetFromJsonAsync<List<JobApplicationDto>>("/api/job-applications", cancellationToken);
            return result ?? new List<JobApplicationDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching all applications");
            return new List<JobApplicationDto>();
        }
    }

    public async Task<List<InterviewDto>> GetInterviewsByApplicationAsync(int applicationId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching interviews for application {ApplicationId}", applicationId);
            var result = await _httpClient.GetFromJsonAsync<List<InterviewDto>>($"/api/interviews/application/{applicationId}", cancellationToken);
            return result ?? new List<InterviewDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching interviews for application {ApplicationId}", applicationId);
            return new List<InterviewDto>();
        }
    }

    public async Task<List<InterviewDto>> GetAllInterviewsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all interviews");
            var result = await _httpClient.GetFromJsonAsync<List<InterviewDto>>("/api/interviews", cancellationToken);
            return result ?? new List<InterviewDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching all interviews");
            return new List<InterviewDto>();
        }
    }
}
