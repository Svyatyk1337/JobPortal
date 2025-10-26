using AutoMapper;
using JobPortal.Application.Bll.DTOs;
using JobPortal.Application.Bll.Interfaces;
using JobPortal.Application.Dal.Interfaces;
using JobPortal.Application.Domain.Exceptions;
using JobPortal.Application.Domain.Models;
using Microsoft.Extensions.Logging;

namespace JobPortal.Application.Bll.Services;

public class CandidateService : ICandidateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CandidateService> _logger;

    public CandidateService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CandidateService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CandidateDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting candidate with ID: {CandidateId}", id);

        var candidate = await _unitOfWork.Candidates.GetByIdAsync(id, cancellationToken);

        return candidate != null ? _mapper.Map<CandidateDto>(candidate) : null;
    }

    public async Task<IEnumerable<CandidateDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Getting all candidates");

        var candidates = await _unitOfWork.Candidates.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<CandidateDto>>(candidates);
    }

    public async Task<CandidateDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ValidationException("Email cannot be empty");
        }

        _logger.LogDebug("Getting candidate with email: {Email}", email);

        var candidate = await _unitOfWork.Candidates.GetByEmailAsync(email, cancellationToken);

        return candidate != null ? _mapper.Map<CandidateDto>(candidate) : null;
    }

    public async Task<CandidateDto> CreateAsync(CreateCandidateDto createDto, CancellationToken cancellationToken = default)
    {
        // Business validation
        ValidateCreateCandidate(createDto);

        // Check if candidate with email already exists
        var existing = await _unitOfWork.Candidates.GetByEmailAsync(createDto.Email, cancellationToken);
        if (existing != null)
        {
            throw new BusinessConflictException($"Candidate with email '{createDto.Email}' already exists");
        }

        _logger.LogInformation("Creating new candidate: {Email}", createDto.Email);

        var candidate = _mapper.Map<Candidate>(createDto);
        candidate.CreatedAt = DateTime.UtcNow;

        var id = await _unitOfWork.Candidates.CreateAsync(candidate, cancellationToken);
        candidate.Id = id;

        return _mapper.Map<CandidateDto>(candidate);
    }

    public async Task<CandidateDto> UpdateAsync(UpdateCandidateDto updateDto, CancellationToken cancellationToken = default)
    {
        // Business validation
        ValidateUpdateCandidate(updateDto);

        // Check if candidate exists
        var existing = await _unitOfWork.Candidates.GetByIdAsync(updateDto.Id, cancellationToken);
        if (existing == null)
        {
            throw new NotFoundException(nameof(Candidate), updateDto.Id);
        }

        // Check if email is taken by another candidate
        var candidateWithEmail = await _unitOfWork.Candidates.GetByEmailAsync(updateDto.Email, cancellationToken);
        if (candidateWithEmail != null && candidateWithEmail.Id != updateDto.Id)
        {
            throw new BusinessConflictException($"Email '{updateDto.Email}' is already taken by another candidate");
        }

        _logger.LogInformation("Updating candidate with ID: {CandidateId}", updateDto.Id);

        var candidate = _mapper.Map<Candidate>(updateDto);
        candidate.CreatedAt = existing.CreatedAt;

        await _unitOfWork.Candidates.UpdateAsync(candidate, cancellationToken);

        return _mapper.Map<CandidateDto>(candidate);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        // Check if candidate exists
        var exists = await _unitOfWork.Candidates.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            throw new NotFoundException(nameof(Candidate), id);
        }

        _logger.LogInformation("Deleting candidate with ID: {CandidateId}", id);

        return await _unitOfWork.Candidates.DeleteAsync(id, cancellationToken);
    }

    private void ValidateCreateCandidate(CreateCandidateDto dto)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(dto.FirstName))
            errors.Add(nameof(dto.FirstName), new[] { "First name is required" });

        if (string.IsNullOrWhiteSpace(dto.LastName))
            errors.Add(nameof(dto.LastName), new[] { "Last name is required" });

        if (string.IsNullOrWhiteSpace(dto.Email))
            errors.Add(nameof(dto.Email), new[] { "Email is required" });

        if (dto.YearsOfExperience < 0)
            errors.Add(nameof(dto.YearsOfExperience), new[] { "Years of experience cannot be negative" });

        if (errors.Any())
            throw new ValidationException(errors);
    }

    private void ValidateUpdateCandidate(UpdateCandidateDto dto)
    {
        var errors = new Dictionary<string, string[]>();

        if (dto.Id <= 0)
            errors.Add(nameof(dto.Id), new[] { "Invalid candidate ID" });

        if (string.IsNullOrWhiteSpace(dto.FirstName))
            errors.Add(nameof(dto.FirstName), new[] { "First name is required" });

        if (string.IsNullOrWhiteSpace(dto.LastName))
            errors.Add(nameof(dto.LastName), new[] { "Last name is required" });

        if (string.IsNullOrWhiteSpace(dto.Email))
            errors.Add(nameof(dto.Email), new[] { "Email is required" });

        if (dto.YearsOfExperience < 0)
            errors.Add(nameof(dto.YearsOfExperience), new[] { "Years of experience cannot be negative" });

        if (errors.Any())
            throw new ValidationException(errors);
    }
}
