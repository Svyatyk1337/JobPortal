using JobPortal.Application.Bll.DTOs;
using JobPortal.Application.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Application.Api.Controllers;

/// <summary>
/// Candidates API Controller
/// Demonstrates attribute routing, async operations, and proper HTTP status codes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CandidatesController : ControllerBase
{
    private readonly ICandidateService _candidateService;
    private readonly ILogger<CandidatesController> _logger;

    public CandidatesController(
        ICandidateService candidateService,
        ILogger<CandidatesController> logger)
    {
        _candidateService = candidateService ?? throw new ArgumentNullException(nameof(candidateService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get all candidates
    /// </summary>
    /// <returns>List of candidates</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CandidateDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CandidateDto>>> GetAll(CancellationToken cancellationToken)
    {
        var candidates = await _candidateService.GetAllAsync(cancellationToken);
        return Ok(candidates);
    }

    /// <summary>
    /// Get candidate by ID
    /// </summary>
    /// <param name="id">Candidate ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Candidate details</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CandidateDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var candidate = await _candidateService.GetByIdAsync(id, cancellationToken);

        if (candidate == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Candidate not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Candidate with ID {id} was not found"
            });
        }

        return Ok(candidate);
    }

    /// <summary>
    /// Get candidate by email
    /// </summary>
    /// <param name="email">Candidate email</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Candidate details</returns>
    [HttpGet("by-email")]
    [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CandidateDto>> GetByEmail(
        [FromQuery] string email,
        CancellationToken cancellationToken)
    {
        var candidate = await _candidateService.GetByEmailAsync(email, cancellationToken);

        if (candidate == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Candidate not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Candidate with email '{email}' was not found"
            });
        }

        return Ok(candidate);
    }

    /// <summary>
    /// Create a new candidate
    /// </summary>
    /// <param name="createDto">Candidate creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created candidate with Location header</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CandidateDto>> Create(
        [FromBody] CreateCandidateDto createDto,
        CancellationToken cancellationToken)
    {
        var candidate = await _candidateService.CreateAsync(createDto, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = candidate.Id },
            candidate);
    }

    /// <summary>
    /// Update existing candidate
    /// </summary>
    /// <param name="id">Candidate ID</param>
    /// <param name="updateDto">Candidate update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated candidate</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CandidateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CandidateDto>> Update(
        int id,
        [FromBody] UpdateCandidateDto updateDto,
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

        var candidate = await _candidateService.UpdateAsync(updateDto, cancellationToken);
        return Ok(candidate);
    }

    /// <summary>
    /// Delete candidate
    /// </summary>
    /// <param name="id">Candidate ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _candidateService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
