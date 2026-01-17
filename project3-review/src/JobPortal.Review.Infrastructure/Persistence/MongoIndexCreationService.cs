using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Domain.Entities;
using MongoDB.Driver;

namespace JobPortal.Review.Infrastructure.Persistence;

public class MongoIndexCreationService : IMongoIndexCreationService
{
    private readonly MongoDbContext _context;

    public MongoIndexCreationService(MongoDbContext context)
    {
        _context = context;
    }

    public async Task CreateIndexesAsync(CancellationToken cancellationToken = default)
    {
        var indexKeysDefinitions = new List<CreateIndexModel<CompanyReview>>();

        // 1. Index on company_id for fast company-based queries
        var companyIdIndex = Builders<CompanyReview>.IndexKeys.Ascending(r => r.CompanyId);
        indexKeysDefinitions.Add(new CreateIndexModel<CompanyReview>(
            companyIdIndex,
            new CreateIndexOptions { Name = "idx_company_id" }
        ));

        // 2. Index on user_id for fast user-based queries
        var userIdIndex = Builders<CompanyReview>.IndexKeys.Ascending(r => r.UserId);
        indexKeysDefinitions.Add(new CreateIndexModel<CompanyReview>(
            userIdIndex,
            new CreateIndexOptions { Name = "idx_user_id" }
        ));

        // 3. Compound index on (company_id, created_at) for sorted company reviews
        var companyCreatedAtIndex = Builders<CompanyReview>.IndexKeys
            .Ascending(r => r.CompanyId)
            .Descending(r => r.CreatedAt);
        indexKeysDefinitions.Add(new CreateIndexModel<CompanyReview>(
            companyCreatedAtIndex,
            new CreateIndexOptions { Name = "idx_company_id_created_at" }
        ));

        // 4. Text index on review_text, title, pros, and cons for full-text search
        var textIndex = Builders<CompanyReview>.IndexKeys
            .Text(r => r.ReviewText)
            .Text(r => r.Title)
            .Text(r => r.Pros)
            .Text(r => r.Cons);
        indexKeysDefinitions.Add(new CreateIndexModel<CompanyReview>(
            textIndex,
            new CreateIndexOptions { Name = "idx_text_search" }
        ));

        // Create all indexes at once
        await _context.CompanyReviews.Indexes.CreateManyAsync(
            indexKeysDefinitions,
            cancellationToken
        );
    }
}
