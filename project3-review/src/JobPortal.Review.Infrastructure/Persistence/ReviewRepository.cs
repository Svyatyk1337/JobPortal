using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Application.Common.Models;
using JobPortal.Review.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JobPortal.Review.Infrastructure.Persistence;

public class ReviewRepository : MongoRepository<CompanyReview>, IReviewRepository
{
    public ReviewRepository(IMongoCollection<CompanyReview> collection) : base(collection)
    {
    }

    public async Task<double?> GetAverageRatingByCompanyAsync(int companyId, CancellationToken cancellationToken = default)
    {
        // Aggregation Pipeline:
        // 1. $match - filter by companyId
        // 2. $group - calculate average of overall_rating.Value
        // 3. $project - return only the average

        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument("company_id", companyId)),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "avgRating", new BsonDocument("$avg", "$overall_rating.Value") }
            }),
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "avgRating", 1 }
            })
        };

        var result = await _collection.Aggregate<BsonDocument>(pipeline, cancellationToken: cancellationToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null || !result.Contains("avgRating"))
        {
            return null;
        }

        return result["avgRating"].ToDouble();
    }

    public async Task<ReviewsStats> GetReviewsStatsByCompanyAsync(int companyId, CancellationToken cancellationToken = default)
    {
        // Aggregation Pipeline:
        // 1. $match - filter by companyId
        // 2. $group - calculate count, average, and collect all ratings
        // 3. $project - format the output

        var matchStage = new BsonDocument("$match", new BsonDocument("company_id", companyId));

        var groupStage = new BsonDocument("$group", new BsonDocument
        {
            { "_id", BsonNull.Value },
            { "count", new BsonDocument("$sum", 1) },
            { "avgRating", new BsonDocument("$avg", "$overall_rating.Value") },
            { "ratings", new BsonDocument("$push", "$overall_rating.Value") }
        });

        var projectStage = new BsonDocument("$project", new BsonDocument
        {
            { "_id", 0 },
            { "count", 1 },
            { "avgRating", 1 },
            { "ratings", 1 }
        });

        var pipeline = new[] { matchStage, groupStage, projectStage };

        var result = await _collection.Aggregate<BsonDocument>(pipeline, cancellationToken: cancellationToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            return new ReviewsStats
            {
                Count = 0,
                AverageRating = 0,
                RatingDistribution = new Dictionary<int, int>()
            };
        }

        var count = result["count"].ToInt32();
        var avgRating = result["avgRating"].ToDouble();
        var ratings = result["ratings"].AsBsonArray;

        // Calculate rating distribution (1-5 stars)
        var ratingDistribution = new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 }
        };

        foreach (var rating in ratings)
        {
            var ratingValue = (int)Math.Round(rating.ToDouble());
            if (ratingValue >= 1 && ratingValue <= 5)
            {
                ratingDistribution[ratingValue]++;
            }
        }

        return new ReviewsStats
        {
            Count = count,
            AverageRating = Math.Round(avgRating, 2),
            RatingDistribution = ratingDistribution
        };
    }

    public async Task<IEnumerable<CompanyReview>> SearchReviewsAsync(string searchText, int skip, int limit, CancellationToken cancellationToken = default)
    {
        // Aggregation Pipeline:
        // 1. $match - text search using $text operator
        // 2. $sort - by text score (relevance)
        // 3. $skip - pagination
        // 4. $limit - pagination

        // Note: This requires a text index to be created on the fields you want to search
        // Text index should be created on: title, review_text, pros, cons

        var matchStage = new BsonDocument("$match", new BsonDocument("$text",
            new BsonDocument("$search", searchText)));

        var sortStage = new BsonDocument("$sort",
            new BsonDocument("score", new BsonDocument("$meta", "textScore")));

        var skipStage = new BsonDocument("$skip", skip);
        var limitStage = new BsonDocument("$limit", limit);

        var pipeline = new[] { matchStage, sortStage, skipStage, limitStage };

        var results = await _collection.Aggregate<CompanyReview>(pipeline, cancellationToken: cancellationToken)
            .ToListAsync(cancellationToken);

        return results;
    }
}
