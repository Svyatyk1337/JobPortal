using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Domain.Entities;
using JobPortal.Review.Domain.ValueObjects;
using JobPortal.Review.Infrastructure.Persistence;
using JobPortal.Review.Infrastructure.Persistence.Serializers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace JobPortal.Review.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register MongoDB settings
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        // Register custom BSON serializers and class maps
        if (!BsonClassMap.IsClassMapRegistered(typeof(Rating)))
        {
            BsonSerializer.RegisterSerializer(new RatingSerializer());
            MoneyClassMap.Register();
            LocationClassMap.Register();
        }

        // Register MongoDbContext
        services.AddSingleton<MongoDbContext>();

        // Register repositories
        services.AddScoped<IRepository<CompanyReview>>(sp =>
        {
            var context = sp.GetRequiredService<MongoDbContext>();
            return new MongoRepository<CompanyReview>(context.CompanyReviews);
        });

        // Register MongoDB services
        services.AddScoped<IMongoIndexCreationService, MongoIndexCreationService>();
        services.AddScoped<IDataSeeder, DataSeeder>();

        return services;
    }
}
