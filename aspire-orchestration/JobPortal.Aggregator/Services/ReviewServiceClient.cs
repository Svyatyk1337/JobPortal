using System.Net.Http.Json;
using JobPortal.Aggregator.DTOs;

namespace JobPortal.Aggregator.Services;

public class ReviewServiceClient : IReviewServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReviewServiceClient> _logger;

    public ReviewServiceClient(HttpClient httpClient, ILogger<ReviewServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<CompanyReviewDto>> GetReviewsByCompanyAsync(int companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching reviews for company {CompanyId}", companyId);
            var result = await _httpClient.GetFromJsonAsync<List<CompanyReviewDto>>($"/api/company-reviews/company/{companyId}", cancellationToken);
            return result ?? new List<CompanyReviewDto>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching reviews for company {CompanyId}", companyId);
            return new List<CompanyReviewDto>();
        }
    }

    public async Task<CompanyReviewDto?> GetReviewAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Fetching review with ID {ReviewId}", id);
            return await _httpClient.GetFromJsonAsync<CompanyReviewDto>($"/api/company-reviews/{id}", cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching review {ReviewId}", id);
            return null;
        }
    }
}
