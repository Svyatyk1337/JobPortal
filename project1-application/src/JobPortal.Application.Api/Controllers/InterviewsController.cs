using JobPortal.Application.Bll.DTOs;
using JobPortal.Application.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Application.Api.Controllers;

/// <summary>
/// Interviews API Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class InterviewsController : ControllerBase
{
    private readonly IInterviewService _interviewService;
    private readonly ILogger<InterviewsController> _logger;

    public InterviewsController(
        IInterviewService interviewService,
        ILogger<InterviewsController> logger)
    {
        _interviewService = interviewService ?? throw new ArgumentNullException(nameof(interviewService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get interview by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InterviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InterviewDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var interview = await _interviewService.GetByIdAsync(id, cancellationToken);

        if (interview == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Interview not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Interview with ID {id} was not found"
            });
        }

        return Ok(interview);
    }

    /// <summary>
    /// Get all interviews for a specific job application
    /// </summary>
    [HttpGet("by-application/{applicationId:int}")]
    [ProducesResponseType(typeof(IEnumerable<InterviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InterviewDto>>> GetByApplicationId(
        int applicationId,
        CancellationToken cancellationToken)
    {
        var interviews = await _interviewService.GetByApplicationIdAsync(applicationId, cancellationToken);
        return Ok(interviews);
    }

    /// <summary>
    /// Get all upcoming interviews
    /// </summary>
    [HttpGet("upcoming")]
    [ProducesResponseType(typeof(IEnumerable<InterviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InterviewDto>>> GetUpcoming(CancellationToken cancellationToken)
    {
        var interviews = await _interviewService.GetUpcomingInterviewsAsync(cancellationToken);
        return Ok(interviews);
    }

    /// <summary>
    /// Schedule a new interview
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InterviewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InterviewDto>> Create(
        [FromBody] CreateInterviewDto createDto,
        CancellationToken cancellationToken)
    {
        var interview = await _interviewService.CreateAsync(createDto, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = interview.Id },
            interview);
    }

    /// <summary>
    /// Update interview details
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(InterviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InterviewDto>> Update(
        int id,
        [FromBody] UpdateInterviewDto updateDto,
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

        var interview = await _interviewService.UpdateAsync(updateDto, cancellationToken);
        return Ok(interview);
    }

    /// <summary>
    /// Cancel/delete interview
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _interviewService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
