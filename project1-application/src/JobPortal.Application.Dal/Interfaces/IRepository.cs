using System.Data;

namespace JobPortal.Application.Dal.Interfaces;

/// <summary>
/// Generic Repository interface for ADO.NET based data access
/// Provides base CRUD operations for all entities
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
/// <typeparam name="TKey">Primary key type</typeparam>
public interface IRepository<T, TKey> where T : class
{
    /// <summary>
    /// Gets an entity by its primary key
    /// </summary>
    Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new entity and returns its ID
    /// </summary>
    Task<TKey> CreateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity by its primary key
    /// </summary>
    Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an entity exists by its primary key
    /// </summary>
    Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the transaction context for the repository
    /// </summary>
    void SetTransaction(IDbConnection connection, IDbTransaction transaction);
}
