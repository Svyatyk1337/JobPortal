var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL for Application Service (Project 1)
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var applicationDb = postgres.AddDatabase("ApplicationDb");

// Add MySQL for Catalog Service (Project 2)
var mysql = builder.AddMySql("mysql")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = mysql.AddDatabase("CatalogDb");

// Add MongoDB for Review Service (Project 3)
var mongodb = builder.AddMongoDB("mongodb")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var reviewDb = mongodb.AddDatabase("review_db");

// Add Redis for distributed caching
var redis = builder.AddRedis("cache")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

// Add Application Service (Project 1 - PostgreSQL)
var applicationService = builder.AddProject<Projects.JobPortal_Application_Api>("application-service")
    .WithReference(applicationDb)
    .WaitFor(applicationDb);

// Add Catalog Service (Project 2 - MySQL)
var catalogService = builder.AddProject<Projects.JobPortal_Catalog_WebApi>("catalog-service")
    .WithReference(catalogDb)
    .WithReference(redis)
    .WaitFor(catalogDb)
    .WaitFor(redis);

// Add Review Service (Project 3 - MongoDB)
var reviewService = builder.AddProject<Projects.JobPortal_Review_WebApi>("review-service")
    .WithReference(reviewDb)
    .WaitFor(reviewDb);

// Add Aggregator Service
var aggregator = builder.AddProject<Projects.JobPortal_Aggregator>("aggregator")
    .WithReference(applicationService)
    .WithReference(catalogService)
    .WithReference(reviewService)
    .WaitFor(applicationService)
    .WaitFor(catalogService)
    .WaitFor(reviewService);

// Add API Gateway (YARP)
var gateway = builder.AddProject<Projects.JobPortal_ApiGateway>("gateway")
    .WithExternalHttpEndpoints()
    .WithReference(aggregator)
    .WithReference(applicationService)
    .WithReference(catalogService)
    .WithReference(reviewService)
    .WaitFor(aggregator)
    .WaitFor(applicationService)
    .WaitFor(catalogService)
    .WaitFor(reviewService);

builder.Build().Run();
