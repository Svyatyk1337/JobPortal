using AutoMapper;
using FluentValidation;
using JobPortal.Catalog.Bll.DTOs;
using JobPortal.Catalog.Data.UnitOfWork;
using JobPortal.Catalog.Domain.Entities;
using JobPortal.Catalog.Domain.Exceptions;

namespace JobPortal.Catalog.Bll.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCompanyDto> _createValidator;
    private readonly IValidator<UpdateCompanyDto> _updateValidator;

    public CompanyService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateCompanyDto> createValidator,
        IValidator<UpdateCompanyDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var companies = await _unitOfWork.Companies.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CompanyDto>>(companies);
    }

    public async Task<CompanyDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(id, cancellationToken);
        return company == null ? null : _mapper.Map<CompanyDto>(company);
    }

    public async Task<CompanyWithContactDto?> GetWithContactAsync(int id, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetWithContactAsync(id, cancellationToken);
        return company == null ? null : _mapper.Map<CompanyWithContactDto>(company);
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyDto createDto, CancellationToken cancellationToken = default)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        // Check for duplicate company name (409 Conflict)
        var existingCompanies = await _unitOfWork.Companies.FindAsync(
            c => c.Name == createDto.Name,
            cancellationToken);

        if (existingCompanies.Any())
        {
            throw new Domain.Exceptions.ConflictException(nameof(Company), nameof(Company.Name), createDto.Name);
        }

        var company = _mapper.Map<Company>(createDto);
        await _unitOfWork.Companies.AddAsync(company, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CompanyDto>(company);
    }

    public async Task UpdateAsync(int id, UpdateCompanyDto updateDto, CancellationToken cancellationToken = default)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var company = await _unitOfWork.Companies.GetByIdAsync(id, cancellationToken);
        if (company == null)
        {
            throw new NotFoundException(nameof(Company), id);
        }

        _mapper.Map(updateDto, company);
        await _unitOfWork.Companies.UpdateAsync(company, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var company = await _unitOfWork.Companies.GetByIdAsync(id, cancellationToken);
        if (company == null)
        {
            throw new NotFoundException(nameof(Company), id);
        }

        await _unitOfWork.Companies.DeleteAsync(company, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompanyDto>> GetByIndustryAsync(string industry, CancellationToken cancellationToken = default)
    {
        var companies = await _unitOfWork.Companies.GetByIndustryAsync(industry, cancellationToken);
        return _mapper.Map<IEnumerable<CompanyDto>>(companies);
    }
}
