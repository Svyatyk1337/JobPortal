using JobPortal.Catalog.Bll.DTOs;
using JobPortal.Catalog.Bll.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Catalog.WebApi.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;
    private readonly ILogger<JobsController> _logger;

    public JobsController(IJobService jobService, ILogger<JobsController> logger)
    {
        _jobService = jobService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<JobDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all jobs");
        var jobs = await _jobService.GetAllAsync(cancellationToken);
        return Ok(jobs);
    }

    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<JobDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetActive(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all active jobs");
        var jobs = await _jobService.GetActiveJobsAsync(cancellationToken);
        return Ok(jobs);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobDto>> GetById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting job with ID {JobId}", id);
        var job = await _jobService.GetByIdAsync(id, cancellationToken);

        if (job == null)
        {
            return NotFound($"Job with ID {id} not found");
        }

        return Ok(job);
    }

    [HttpGet("{id}/details")]
    [ProducesResponseType(typeof(JobWithDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobWithDetailsDto>> GetWithDetails(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting job with details for ID {JobId}", id);
        var job = await _jobService.GetWithDetailsAsync(id, cancellationToken);

        if (job == null)
        {
            return NotFound($"Job with ID {id} not found");
        }

        return Ok(job);
    }

    [HttpGet("company/{companyId}")]
    [ProducesResponseType(typeof(IEnumerable<JobDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetByCompany(int companyId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting jobs for company {CompanyId}", companyId);
        var jobs = await _jobService.GetJobsByCompanyAsync(companyId, cancellationToken);
        return Ok(jobs);
    }

    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(typeof(IEnumerable<JobDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetByCategory(int categoryId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting jobs for category {CategoryId}", categoryId);
        var jobs = await _jobService.GetJobsByCategoryAsync(categoryId, cancellationToken);
        return Ok(jobs);
    }

    [HttpPost]
    [ProducesResponseType(typeof(JobDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JobDto>> Create([FromBody] CreateJobDto createDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new job");
        var job = await _jobService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateJobDto updateDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating job with ID {JobId}", id);
        await _jobService.UpdateAsync(id, updateDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting job with ID {JobId}", id);
        await _jobService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
