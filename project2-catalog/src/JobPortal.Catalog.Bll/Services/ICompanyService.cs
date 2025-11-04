using JobPortal.Catalog.Bll.DTOs;

namespace JobPortal.Catalog.Bll.Services;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CompanyDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CompanyWithContactDto?> GetWithContactAsync(int id, CancellationToken cancellationToken = default);
    Task<CompanyDto> CreateAsync(CreateCompanyDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateCompanyDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyDto>> GetByIndustryAsync(string industry, CancellationToken cancellationToken = default);
}
