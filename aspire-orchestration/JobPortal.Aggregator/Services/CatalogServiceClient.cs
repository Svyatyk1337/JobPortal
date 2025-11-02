using System.Net.Http.Json;
using JobPortal.Aggregator.DTOs;

namespace JobPortal.Aggregator.Services;

public class CatalogServiceClient : ICatalogServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatalogServiceClient> _logger;

    public CatalogServiceClient(HttpClient httpClient, ILogger<CatalogServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<JobDto?> GetJobAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching job with ID {JobId}", id);
            return await _httpClient.GetFromJsonAsync<JobDto>($"/api/jobs/{id}", cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching job {JobId}", id);
            return null;
        }
    }

    public async Task<List<JobDto>> GetAllJobsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all jobs");
            var result = await _httpClient.GetFromJsonAsync<List<JobDto>>("/api/jobs", cancellationToken);
            return result ?? new List<JobDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching all jobs");
            return new List<JobDto>();
        }
    }

    public async Task<CompanyDto?> GetCompanyAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching company with ID {CompanyId}", id);
            return await _httpClient.GetFromJsonAsync<CompanyDto>($"/api/companies/{id}", cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching company {CompanyId}", id);
            return null;
        }
    }

    public async Task<List<CompanyDto>> GetAllCompaniesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching all companies");
            var result = await _httpClient.GetFromJsonAsync<List<CompanyDto>>("/api/companies", cancellationToken);
            return result ?? new List<CompanyDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching all companies");
            return new List<CompanyDto>();
        }
    }

    public async Task<List<JobDto>> GetJobsByCompanyAsync(int companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching jobs for company {CompanyId}", companyId);
            var result = await _httpClient.GetFromJsonAsync<List<JobDto>>($"/api/jobs/company/{companyId}", cancellationToken);
            return result ?? new List<JobDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching jobs for company {CompanyId}", companyId);
            return new List<JobDto>();
        }
    }
}
