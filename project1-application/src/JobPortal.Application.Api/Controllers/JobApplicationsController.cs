using JobPortal.Application.Bll.DTOs;
using JobPortal.Application.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Application.Api.Controllers;

/// <summary>
/// Job Applications API Controller
/// Demonstrates transactional operations through BLL services
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _jobApplicationService;
    private readonly ILogger<JobApplicationsController> _logger;

    public JobApplicationsController(
        IJobApplicationService jobApplicationService,
        ILogger<JobApplicationsController> logger)
    {
        _jobApplicationService = jobApplicationService ?? throw new ArgumentNullException(nameof(jobApplicationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all job applications
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<JobApplicationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetAll(CancellationToken cancellationToken)
    {
        var applications = await _jobApplicationService.GetAllAsync(cancellationToken);
        return Ok(applications);
    }

    /// <summary>
    /// Get job application by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(JobApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var application = await _jobApplicationService.GetByIdAsync(id, cancellationToken);

        if (application == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Job application not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Job application with ID {id} was not found"
            });
        }

        return Ok(application);
    }

    /// <summary>
    /// Get job application with full details (includes candidate, application details)
    /// </summary>
    [HttpGet("{id:int}/details")]
    [ProducesResponseType(typeof(JobApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationDto>> GetByIdWithDetails(int id, CancellationToken cancellationToken)
    {
        var application = await _jobApplicationService.GetByIdWithDetailsAsync(id, cancellationToken);

        if (application == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Job application not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Job application with ID {id} was not found"
            });
        }

        return Ok(application);
    }

    /// <summary>
    /// Get all job applications for a specific candidate
    /// </summary>
    [HttpGet("by-candidate/{candidateId:int}")]
    [ProducesResponseType(typeof(IEnumerable<JobApplicationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetByCandidateId(
        int candidateId,
        CancellationToken cancellationToken)
    {
        var applications = await _jobApplicationService.GetByCandidateIdAsync(candidateId, cancellationToken);
        return Ok(applications);
    }

    /// <summary>
    /// Get all job applications by status
    /// </summary>
    [HttpGet("by-status")]
    [ProducesResponseType(typeof(IEnumerable<JobApplicationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<JobApplicationDto>>> GetByStatus(
        [FromQuery] string status,
        CancellationToken cancellationToken)
    {
        var applications = await _jobApplicationService.GetByStatusAsync(status, cancellationToken);
        return Ok(applications);
    }

    /// <summary>
    /// Create new job application with details (transactional operation)
    /// </summary>
    /// <remarks>
    /// This endpoint demonstrates Unit of Work pattern with transaction management.
    /// Both job application and its details are created atomically.
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(JobApplicationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<JobApplicationDto>> Create(
        [FromBody] CreateJobApplicationDto createDto,
        CancellationToken cancellationToken)
    {
        var application = await _jobApplicationService.CreateWithDetailsAsync(createDto, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = application.Id },
            application);
    }

    /// <summary>
    /// Update existing job application
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(JobApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<JobApplicationDto>> Update(
        int id,
        [FromBody] UpdateJobApplicationDto updateDto,
        CancellationToken cancellationToken)
    {
        if (id != updateDto.Id)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "ID mismatch",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The ID in the URL does not match the ID in the request body"
            });
        }

        var application = await _jobApplicationService.UpdateAsync(updateDto, cancellationToken);
        return Ok(application);
    }

    /// <summary>
    /// Delete job application
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _jobApplicationService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
