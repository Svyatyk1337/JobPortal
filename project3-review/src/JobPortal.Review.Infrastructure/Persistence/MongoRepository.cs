using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Domain.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace JobPortal.Review.Infrastructure.Persistence;

public class MongoRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        // Validate if the id is a valid ObjectId format
        if (!ObjectId.TryParse(id, out _))
        {
            return null;
        }

        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(predicate).ToListAsync(cancellationToken);
    }

    public async Task<string> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(entity, null, cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(string id, T entity, CancellationToken cancellationToken = default)
    {
        // Validate if the id is a valid ObjectId format
        if (!ObjectId.TryParse(id, out _))
        {
            throw new ArgumentException($"Invalid ObjectId format: {id}", nameof(id));
        }

        await _collection.ReplaceOneAsync(x => x.Id == id, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        // Validate if the id is a valid ObjectId format
        if (!ObjectId.TryParse(id, out _))
        {
            throw new ArgumentException($"Invalid ObjectId format: {id}", nameof(id));
        }

        await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<long> CountAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<T>> GetPagedAsync(
        FilterDefinition<T> filter,
        SortDefinition<T> sort,
        int skip,
        int limit,
        CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(filter)
            .Sort(sort)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync(cancellationToken);
    }
}
