using JobPortal.Application.Bll.DTOs;

namespace JobPortal.Application.Bll.Interfaces;

public interface ICandidateService
{
    Task<CandidateDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CandidateDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CandidateDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<CandidateDto> CreateAsync(CreateCandidateDto createDto, CancellationToken cancellationToken = default);
    Task<CandidateDto> UpdateAsync(UpdateCandidateDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
