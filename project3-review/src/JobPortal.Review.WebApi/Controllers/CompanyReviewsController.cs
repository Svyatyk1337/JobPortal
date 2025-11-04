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
}
