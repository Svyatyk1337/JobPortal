using JobPortal.Aggregator.DTOs;
using JobPortal.Aggregator.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Aggregator.Controllers;

[ApiController]
[Route("api/aggregate")]
public class AggregatorController : ControllerBase
{
    private readonly IApplicationServiceClient _applicationClient;
    private readonly ICatalogServiceClient _catalogClient;
    private readonly IReviewServiceClient _reviewClient;
    private readonly ILogger<AggregatorController> _logger;

    public AggregatorController(
        IApplicationServiceClient applicationClient,
        ICatalogServiceClient catalogClient,
        IReviewServiceClient reviewClient,
        ILogger<AggregatorController> logger)
    {
        _applicationClient = applicationClient;
        _catalogClient = catalogClient;
        _reviewClient = reviewClient;
        _logger = logger;
    }

    /// <summary>
    /// Get complete candidate profile with applications and interviews
    /// </summary>
    [HttpGet("candidate/{id}")]
    [ProducesResponseType(typeof(CandidateProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CandidateProfileDto>> GetCandidateProfile(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Aggregating candidate profile for candidate {CandidateId}", id);

        var candidateTask = _applicationClient.GetCandidateAsync(id, cancellationToken);
        var applicationsTask = _applicationClient.GetApplicationsByCandidateAsync(id, cancellationToken);

        await Task.WhenAll(candidateTask, applicationsTask);

        var candidate = await candidateTask;
        if (candidate == null)
        {
            return NotFound($"Candidate with ID {id} not found");
        }

        var applications = await applicationsTask;

        // Fetch all interviews for all applications in parallel
        var interviewTasks = applications
            .Select(app => _applicationClient.GetInterviewsByApplicationAsync(app.Id, cancellationToken))
            .ToList();

        var interviewsResults = await Task.WhenAll(interviewTasks);
        var allInterviews = interviewsResults.SelectMany(i => i).ToList();

        return Ok(new CandidateProfileDto
        {
            Candidate = candidate,
            Applications = applications,
            Interviews = allInterviews
        });
    }

    /// <summary>
    /// Get complete job details with company info and reviews
    /// </summary>
    [HttpGet("job/{id}")]
    [ProducesResponseType(typeof(JobDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobDetailDto>> GetJobDetail(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Aggregating job details for job {JobId}", id);

        var job = await _catalogClient.GetJobAsync(id, cancellationToken);
        if (job == null)
        {
            return NotFound($"Job with ID {id} not found");
        }

        var companyTask = _catalogClient.GetCompanyAsync(job.CompanyId, cancellationToken);
        var reviewsTask = _reviewClient.GetReviewsByCompanyAsync(job.CompanyId, cancellationToken);
        var applicationsTask = _applicationClient.GetAllApplicationsAsync(cancellationToken);

        await Task.WhenAll(companyTask, reviewsTask, applicationsTask);

        var company = await companyTask;
        var reviews = await reviewsTask;
        var applications = await applicationsTask;

        var jobApplicationCount = applications.Count(a => a.JobId == id);

        return Ok(new JobDetailDto
        {
            Job = job,
            Company = company ?? new CompanyDto(),
            CompanyReviews = reviews,
            TotalApplications = jobApplicationCount
        });
    }

    /// <summary>
    /// Get complete application details with candidate, job, company, and interviews
    /// </summary>
    [HttpGet("application/{id}")]
    [ProducesResponseType(typeof(ApplicationDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationDetailDto>> GetApplicationDetail(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Aggregating application details for application {ApplicationId}", id);

        var application = await _applicationClient.GetApplicationAsync(id, cancellationToken);
        if (application == null)
        {
            return NotFound($"Application with ID {id} not found");
        }

        var candidateTask = _applicationClient.GetCandidateAsync(application.CandidateId, cancellationToken);
        var jobTask = _catalogClient.GetJobAsync(application.JobId, cancellationToken);
        var interviewsTask = _applicationClient.GetInterviewsByApplicationAsync(id, cancellationToken);

        await Task.WhenAll(candidateTask, jobTask, interviewsTask);

        var candidate = await candidateTask;
        var job = await jobTask;
        var interviews = await interviewsTask;

        CompanyDto? company = null;
        if (job != null)
        {
            company = await _catalogClient.GetCompanyAsync(job.CompanyId, cancellationToken);
        }

        return Ok(new ApplicationDetailDto
        {
            Application = application,
            Candidate = candidate ?? new CandidateDto(),
            Job = job ?? new JobDto(),
            Company = company ?? new CompanyDto(),
            Interviews = interviews
        });
    }

    /// <summary>
    /// Get dashboard overview with statistics and recent data
    /// </summary>
    [HttpGet("dashboard")]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardDto>> GetDashboard(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Aggregating dashboard data");

        var candidatesTask = _applicationClient.GetAllCandidatesAsync(cancellationToken);
        var jobsTask = _catalogClient.GetAllJobsAsync(cancellationToken);
        var applicationsTask = _applicationClient.GetAllApplicationsAsync(cancellationToken);
        var companiesTask = _catalogClient.GetAllCompaniesAsync(cancellationToken);
        var interviewsTask = _applicationClient.GetAllInterviewsAsync(cancellationToken);

        await Task.WhenAll(candidatesTask, jobsTask, applicationsTask, companiesTask, interviewsTask);

        var candidates = await candidatesTask;
        var jobs = await jobsTask;
        var applications = await applicationsTask;
        var companies = await companiesTask;
        var interviews = await interviewsTask;

        var recentApplications = applications
            .OrderByDescending(a => a.AppliedAt)
            .Take(10)
            .ToList();

        var upcomingInterviews = interviews
            .Where(i => i.ScheduledAt > DateTime.UtcNow && i.Status == "Scheduled")
            .OrderBy(i => i.ScheduledAt)
            .Take(10)
            .ToList();

        return Ok(new DashboardDto
        {
            TotalCandidates = candidates.Count,
            TotalJobs = jobs.Count,
            TotalApplications = applications.Count,
            TotalCompanies = companies.Count,
            RecentApplications = recentApplications,
            UpcomingInterviews = upcomingInterviews
        });
    }
}
