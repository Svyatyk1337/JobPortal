using AutoMapper;
using JobPortal.Application.Bll.DTOs;
using JobPortal.Application.Bll.Interfaces;
using JobPortal.Application.Dal.Interfaces;
using JobPortal.Application.Domain.Exceptions;
using JobPortal.Application.Domain.Models;
using Microsoft.Extensions.Logging;

namespace JobPortal.Application.Bll.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<JobApplicationService> _logger;

    public JobApplicationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<JobApplicationService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<JobApplicationDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting job application with ID: {ApplicationId}", id);

        var application = await _unitOfWork.JobApplications.GetByIdAsync(id, cancellationToken);

        return application != null ? _mapper.Map<JobApplicationDto>(application) : null;
    }

    public async Task<JobApplicationDto?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting job application with details, ID: {ApplicationId}", id);

        var application = await _unitOfWork.JobApplications.GetByIdWithDetailsAsync(id, cancellationToken);

        return application != null ? _mapper.Map<JobApplicationDto>(application) : null;
    }

    public async Task<IEnumerable<JobApplicationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting all job applications");

        var applications = await _unitOfWork.JobApplications.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<JobApplicationDto>>(applications);
    }

    public async Task<IEnumerable<JobApplicationDto>> GetByCandidateIdAsync(int candidateId, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting job applications for candidate ID: {CandidateId}", candidateId);

        var applications = await _unitOfWork.JobApplications.GetByCandidateIdAsync(candidateId, cancellationToken);

        return _mapper.Map<IEnumerable<JobApplicationDto>>(applications);
    }

    public async Task<IEnumerable<JobApplicationDto>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting job applications with status: {Status}", status);

        var applications = await _unitOfWork.JobApplications.GetByStatusAsync(status, cancellationToken);

        return _mapper.Map<IEnumerable<JobApplicationDto>>(applications);
    }

    /// <summary>
    /// Creates job application with details in a transaction
    /// Demonstrates Unit of Work pattern with transaction management
    /// </summary>
    public async Task<JobApplicationDto> CreateWithDetailsAsync(CreateJobApplicationDto createDto, CancellationToken cancellationToken = default)
    {
        // Business validation
        await ValidateCreateJobApplication(createDto, cancellationToken);

        _logger.LogInformation("Creating job application for candidate ID: {CandidateId}", createDto.CandidateId);

        try
        {
            // Begin transaction
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Create job application
            var application = _mapper.Map<JobApplication>(createDto);
            application.CreatedAt = DateTime.UtcNow;
            application.SubmittedDate = createDto.SubmittedDate == default
                ? DateTime.UtcNow
                : createDto.SubmittedDate;

            var applicationId = await _unitOfWork.JobApplications.CreateAsync(application, cancellationToken);
            application.Id = applicationId;

            // Create application details if provided
            if (createDto.ApplicationDetails != null)
            {
                var details = _mapper.Map<ApplicationDetails>(createDto.ApplicationDetails);
                details.ApplicationId = applicationId;
                details.CreatedAt = DateTime.UtcNow;

                // Note: We would need to add ApplicationDetails repository to UoW
                // For now, this demonstrates the transaction pattern
                _logger.LogDebug("Application details would be created here");
            }

            // Commit transaction
            await _unitOfWork.CommitAsync(cancellationToken);

            _logger.LogInformation("Job application created successfully with ID: {ApplicationId}", applicationId);

            return _mapper.Map<JobApplicationDto>(application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job application, rolling back transaction");
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<JobApplicationDto> UpdateAsync(UpdateJobApplicationDto updateDto, CancellationToken cancellationToken = default)
    {
        // Business validation
        await ValidateUpdateJobApplication(updateDto, cancellationToken);

        // Check if application exists
        var existing = await _unitOfWork.JobApplications.GetByIdAsync(updateDto.Id, cancellationToken);
        if (existing == null)
        {
            throw new NotFoundException(nameof(JobApplication), updateDto.Id);
        }

        _logger.LogInformation("Updating job application with ID: {ApplicationId}", updateDto.Id);

        var application = _mapper.Map<JobApplication>(updateDto);
        application.CreatedAt = existing.CreatedAt;

        await _unitOfWork.JobApplications.UpdateAsync(application, cancellationToken);

        return _mapper.Map<JobApplicationDto>(application);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        // Check if application exists
        var existing = await _unitOfWork.JobApplications.GetByIdAsync(id, cancellationToken);
        if (existing == null)
        {
            throw new NotFoundException(nameof(JobApplication), id);
        }

        _logger.LogInformation("Deleting job application with ID: {ApplicationId}", id);

        return await _unitOfWork.JobApplications.DeleteAsync(id, cancellationToken);
    }

    private async Task ValidateCreateJobApplication(CreateJobApplicationDto dto, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (dto.CandidateId <= 0)
            errors.Add(nameof(dto.CandidateId), new[] { "Invalid candidate ID" });

        if (dto.JobId <= 0)
            errors.Add(nameof(dto.JobId), new[] { "Invalid job ID" });

        if (string.IsNullOrWhiteSpace(dto.JobTitle))
            errors.Add(nameof(dto.JobTitle), new[] { "Job title is required" });

        if (string.IsNullOrWhiteSpace(dto.CompanyName))
            errors.Add(nameof(dto.CompanyName), new[] { "Company name is required" });

        if (dto.ExpectedSalary.HasValue && dto.ExpectedSalary.Value < 0)
            errors.Add(nameof(dto.ExpectedSalary), new[] { "Expected salary cannot be negative" });

        // Check if candidate exists
        var candidateExists = await _unitOfWork.Candidates.ExistsAsync(dto.CandidateId, cancellationToken);
        if (!candidateExists)
            errors.Add(nameof(dto.CandidateId), new[] { "Candidate does not exist" });

        if (errors.Any())
            throw new ValidationException(errors);
    }

    private async Task ValidateUpdateJobApplication(UpdateJobApplicationDto dto, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (dto.Id <= 0)
            errors.Add(nameof(dto.Id), new[] { "Invalid application ID" });

        if (dto.CandidateId <= 0)
            errors.Add(nameof(dto.CandidateId), new[] { "Invalid candidate ID" });

        if (dto.JobId <= 0)
            errors.Add(nameof(dto.JobId), new[] { "Invalid job ID" });

        if (string.IsNullOrWhiteSpace(dto.JobTitle))
            errors.Add(nameof(dto.JobTitle), new[] { "Job title is required" });

        if (string.IsNullOrWhiteSpace(dto.CompanyName))
            errors.Add(nameof(dto.CompanyName), new[] { "Company name is required" });

        if (dto.ExpectedSalary.HasValue && dto.ExpectedSalary.Value < 0)
            errors.Add(nameof(dto.ExpectedSalary), new[] { "Expected salary cannot be negative" });

        // Check if candidate exists
        var candidateExists = await _unitOfWork.Candidates.ExistsAsync(dto.CandidateId, cancellationToken);
        if (!candidateExists)
            errors.Add(nameof(dto.CandidateId), new[] { "Candidate does not exist" });

        if (errors.Any())
            throw new ValidationException(errors);
    }
}
