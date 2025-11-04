using JobPortal.Review.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JobPortal.Review.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<CompanyReview> CompanyReviews =>
        _database.GetCollection<CompanyReview>("company_reviews");
}

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}
