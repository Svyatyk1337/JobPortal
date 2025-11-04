using AutoMapper;
using FluentValidation;
using JobPortal.Catalog.Bll.DTOs;
using JobPortal.Catalog.Data.UnitOfWork;
using JobPortal.Catalog.Domain.Entities;
using JobPortal.Catalog.Domain.Exceptions;

namespace JobPortal.Catalog.Bll.Services;

public class JobService : IJobService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateJobDto> _createValidator;
    private readonly IValidator<UpdateJobDto> _updateValidator;

    public JobService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateJobDto> createValidator,
        IValidator<UpdateJobDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<JobDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var jobs = await _unitOfWork.Jobs.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<JobDto>>(jobs);
    }

    public async Task<JobDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(id, cancellationToken);
        return job == null ? null : _mapper.Map<JobDto>(job);
    }

    public async Task<JobWithDetailsDto?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var job = await _unitOfWork.Jobs.GetWithCompanyAndCategoryAsync(id, cancellationToken);
        if (job == null) return null;

        // Load skills separately
        var jobWithSkills = await _unitOfWork.Jobs.GetWithSkillsAsync(id, cancellationToken);

        var result = _mapper.Map<JobWithDetailsDto>(job);
        if (jobWithSkills != null)
        {
            result.Skills = _mapper.Map<List<SkillRequirementDto>>(jobWithSkills.SkillRequirements);
        }

        return result;
    }

    public async Task<JobDto> CreateAsync(CreateJobDto createDto, CancellationToken cancellationToken = default)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        // Verify company exists
        var company = await _unitOfWork.Companies.GetByIdAsync(createDto.CompanyId, cancellationToken);
        if (company == null)
        {
            throw new NotFoundException(nameof(Company), createDto.CompanyId);
        }

        // Verify category exists
        var category = await _unitOfWork.JobCategories.GetByIdAsync(createDto.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new NotFoundException(nameof(JobCategory), createDto.CategoryId);
        }

        var job = _mapper.Map<Job>(createDto);
        await _unitOfWork.Jobs.AddAsync(job, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<JobDto>(job);
    }

    public async Task UpdateAsync(int id, UpdateJobDto updateDto, CancellationToken cancellationToken = default)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        var job = await _unitOfWork.Jobs.GetByIdAsync(id, cancellationToken);
        if (job == null)
        {
            throw new NotFoundException(nameof(Job), id);
        }

        _mapper.Map(updateDto, job);
        await _unitOfWork.Jobs.UpdateAsync(job, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var job = await _unitOfWork.Jobs.GetByIdAsync(id, cancellationToken);
        if (job == null)
        {
            throw new NotFoundException(nameof(Job), id);
        }

        await _unitOfWork.Jobs.DeleteAsync(job, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<JobDto>> GetActiveJobsAsync(CancellationToken cancellationToken = default)
    {
        var jobs = await _unitOfWork.Jobs.GetActiveJobsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<JobDto>>(jobs);
    }

    public async Task<IEnumerable<JobDto>> GetJobsByCompanyAsync(int companyId, CancellationToken cancellationToken = default)
    {
        var jobs = await _unitOfWork.Jobs.GetJobsByCompanyAsync(companyId, cancellationToken);
        return _mapper.Map<IEnumerable<JobDto>>(jobs);
    }

    public async Task<IEnumerable<JobDto>> GetJobsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var jobs = await _unitOfWork.Jobs.GetJobsByCategoryAsync(categoryId, cancellationToken);
        return _mapper.Map<IEnumerable<JobDto>>(jobs);
    }
}
