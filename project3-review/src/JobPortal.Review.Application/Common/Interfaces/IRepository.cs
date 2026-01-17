using JobPortal.Review.Application.Common.Models;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace JobPortal.Review.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<string> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(string id, T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<long> CountAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetPagedAsync(FilterDefinition<T> filter, SortDefinition<T> sort, int skip, int limit, CancellationToken cancellationToken = default);
}

public interface IReviewRepository : IRepository<Domain.Entities.CompanyReview>
{
    Task<double?> GetAverageRatingByCompanyAsync(int companyId, CancellationToken cancellationToken = default);
    Task<ReviewsStats> GetReviewsStatsByCompanyAsync(int companyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.CompanyReview>> SearchReviewsAsync(string searchText, int skip, int limit, CancellationToken cancellationToken = default);
}
