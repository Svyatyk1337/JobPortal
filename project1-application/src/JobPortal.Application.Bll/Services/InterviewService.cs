using AutoMapper;
using JobPortal.Application.Bll.DTOs;
using JobPortal.Application.Bll.Interfaces;
using JobPortal.Application.Dal.Interfaces;
using JobPortal.Application.Domain.Exceptions;
using JobPortal.Application.Domain.Models;
using Microsoft.Extensions.Logging;

namespace JobPortal.Application.Bll.Services;

public class InterviewService : IInterviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<InterviewService> _logger;

    public InterviewService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<InterviewService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<InterviewDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting interview with ID: {InterviewId}", id);

        var interview = await _unitOfWork.Interviews.GetByIdAsync(id, cancellationToken);

        return interview != null ? _mapper.Map<InterviewDto>(interview) : null;
    }

    public async Task<IEnumerable<InterviewDto>> GetByApplicationIdAsync(int applicationId, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting interviews for application ID: {ApplicationId}", applicationId);

        var interviews = await _unitOfWork.Interviews.GetByApplicationIdAsync(applicationId, cancellationToken);

        return _mapper.Map<IEnumerable<InterviewDto>>(interviews);
    }

    public async Task<IEnumerable<InterviewDto>> GetUpcomingInterviewsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting upcoming interviews");

        var interviews = await _unitOfWork.Interviews.GetUpcomingInterviewsAsync(cancellationToken);

        return _mapper.Map<IEnumerable<InterviewDto>>(interviews);
    }

    public async Task<InterviewDto> CreateAsync(CreateInterviewDto createDto, CancellationToken cancellationToken = default)
    {
        // Business validation
        await ValidateCreateInterview(createDto, cancellationToken);

        _logger.LogInformation("Creating interview for application ID: {ApplicationId}", createDto.ApplicationId);

        var interview = _mapper.Map<Interview>(createDto);
        interview.CreatedAt = DateTime.UtcNow;

        var id = await _unitOfWork.Interviews.CreateAsync(interview, cancellationToken);
        interview.Id = id;

        return _mapper.Map<InterviewDto>(interview);
    }

    public async Task<InterviewDto> UpdateAsync(UpdateInterviewDto updateDto, CancellationToken cancellationToken = default)
    {
        // Business validation
        await ValidateUpdateInterview(updateDto, cancellationToken);

        // Check if interview exists
        var existing = await _unitOfWork.Interviews.GetByIdAsync(updateDto.Id, cancellationToken);
        if (existing == null)
        {
            throw new NotFoundException(nameof(Interview), updateDto.Id);
        }

        _logger.LogInformation("Updating interview with ID: {InterviewId}", updateDto.Id);

        var interview = _mapper.Map<Interview>(updateDto);
        interview.CreatedAt = existing.CreatedAt;

        await _unitOfWork.Interviews.UpdateAsync(interview, cancellationToken);

        return _mapper.Map<InterviewDto>(interview);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        // Check if interview exists
        var existing = await _unitOfWork.Interviews.GetByIdAsync(id, cancellationToken);
        if (existing == null)
        {
            throw new NotFoundException(nameof(Interview), id);
        }

        _logger.LogInformation("Deleting interview with ID: {InterviewId}", id);

        return await _unitOfWork.Interviews.DeleteAsync(id, cancellationToken);
    }

    private async Task ValidateCreateInterview(CreateInterviewDto dto, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (dto.ApplicationId <= 0)
            errors.Add(nameof(dto.ApplicationId), new[] { "Invalid application ID" });

        if (string.IsNullOrWhiteSpace(dto.InterviewType))
            errors.Add(nameof(dto.InterviewType), new[] { "Interview type is required" });

        if (dto.RoundNumber <= 0)
            errors.Add(nameof(dto.RoundNumber), new[] { "Round number must be positive" });

        if (dto.ScheduledDate < DateTime.UtcNow.AddHours(-1)) // Allow 1 hour buffer
            errors.Add(nameof(dto.ScheduledDate), new[] { "Scheduled date cannot be in the past" });

        // Check if job application exists
        var applicationExists = await _unitOfWork.JobApplications.GetByIdAsync(dto.ApplicationId, cancellationToken);
        if (applicationExists == null)
            errors.Add(nameof(dto.ApplicationId), new[] { "Job application does not exist" });

        if (errors.Any())
            throw new ValidationException(errors);
    }

    private async Task ValidateUpdateInterview(UpdateInterviewDto dto, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>();

        if (dto.Id <= 0)
            errors.Add(nameof(dto.Id), new[] { "Invalid interview ID" });

        if (dto.ApplicationId <= 0)
            errors.Add(nameof(dto.ApplicationId), new[] { "Invalid application ID" });

        if (string.IsNullOrWhiteSpace(dto.InterviewType))
            errors.Add(nameof(dto.InterviewType), new[] { "Interview type is required" });

        if (dto.RoundNumber <= 0)
            errors.Add(nameof(dto.RoundNumber), new[] { "Round number must be positive" });

        // Check if job application exists
        var applicationExists = await _unitOfWork.JobApplications.GetByIdAsync(dto.ApplicationId, cancellationToken);
        if (applicationExists == null)
            errors.Add(nameof(dto.ApplicationId), new[] { "Job application does not exist" });

        if (errors.Any())
            throw new ValidationException(errors);
    }
}
