using JobPortal.Catalog.Bll.DTOs;
using JobPortal.Catalog.Bll.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Catalog.WebApi.Controllers;

[ApiController]
[Route("api/companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly ILogger<CompaniesController> _logger;

    public CompaniesController(ICompanyService companyService, ILogger<CompaniesController> logger)
    {
        _companyService = companyService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CompanyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all companies");
        var companies = await _companyService.GetAllAsync(cancellationToken);
        return Ok(companies);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyDto>> GetById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting company with ID {CompanyId}", id);
        var company = await _companyService.GetByIdAsync(id, cancellationToken);

        if (company == null)
        {
            return NotFound($"Company with ID {id} not found");
        }

        return Ok(company);
    }

    [HttpGet("{id}/with-contact")]
    [ProducesResponseType(typeof(CompanyWithContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyWithContactDto>> GetWithContact(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting company with contact for ID {CompanyId}", id);
        var company = await _companyService.GetWithContactAsync(id, cancellationToken);

        if (company == null)
        {
            return NotFound($"Company with ID {id} not found");
        }

        return Ok(company);
    }

    [HttpGet("industry/{industry}")]
    [ProducesResponseType(typeof(IEnumerable<CompanyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetByIndustry(string industry, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting companies in industry {Industry}", industry);
        var companies = await _companyService.GetByIndustryAsync(industry, cancellationToken);
        return Ok(companies);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CompanyDto>> Create([FromBody] CreateCompanyDto createDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new company");
        var company = await _companyService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyDto updateDto, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating company with ID {CompanyId}", id);
        await _companyService.UpdateAsync(id, updateDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting company with ID {CompanyId}", id);
        await _companyService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
