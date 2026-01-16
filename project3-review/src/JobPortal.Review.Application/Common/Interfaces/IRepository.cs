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
