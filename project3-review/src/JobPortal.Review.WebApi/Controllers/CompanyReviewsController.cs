using JobPortal.Review.Application.Common.Models;
using JobPortal.Review.Application.CompanyReviews.Commands;
using JobPortal.Review.Application.CompanyReviews.DTOs;
using JobPortal.Review.Application.CompanyReviews.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Review.WebApi.Controllers;

[ApiController]
[Route("api/company-reviews")]
public class CompanyReviewsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CompanyReviewsController> _logger;

    public CompanyReviewsController(IMediator mediator, ILogger<CompanyReviewsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompanyReviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CompanyReviewDto>>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all company reviews");

        var query = new GetAllCompanyReviewsQuery();
        var results = await _mediator.Send(query, cancellationToken);

        return Ok(results);
    }

    [HttpGet("paged")]
    [ProducesResponseType(typeof(PagedResult<CompanyReviewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<CompanyReviewDto>>> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? companyId = null,
        [FromQuery] double? minRating = null,
        [FromQuery] double? maxRating = null,
        [FromQuery] bool? isCurrentEmployee = null,
        [FromQuery] string? sortBy = "CreatedAt",
        [FromQuery] bool sortDescending = true,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting paged company reviews");

        var query = new GetCompanyReviewsPagedQuery(
            pageNumber,
            pageSize,
            companyId,
            sortBy);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CompanyReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyReviewDto>> GetById(string id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting company review with ID {ReviewId}", id);

        var query = new GetCompanyReviewByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound($"Company review with ID {id} not found");
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Create([FromBody] CreateCompanyReviewCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new company review");

        var id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCompanyReviewCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating company review with ID {ReviewId}", id);

        if (id != command.Id)
        {
            return BadRequest("ID in URL does not match ID in request body");
        }

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting company review with ID {ReviewId}", id);

        var command = new DeleteCompanyReviewCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
