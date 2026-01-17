using FluentValidation;
using JobPortal.Review.Application.Common.Behaviors;
using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Application.Common.Mappings;
using JobPortal.Review.Infrastructure;
using JobPortal.Review.Infrastructure.Persistence;
using MediatR;
using Serilog;

#if !DEBUG
using JobPortal.ServiceDefaults;
#endif

var builder = WebApplication.CreateBuilder(args);

// ============================================
// Add Aspire Service Defaults (OpenTelemetry, Serilog, Service Discovery)
// ============================================
#if !DEBUG
builder.AddServiceDefaults();
#else
// Configure Serilog for local development
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
#endif

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Job Portal Review API",
        Version = "v1",
        Description = "API for managing reviews using CQRS, MediatR, and MongoDB with Clean Architecture"
    });
});

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add MediatR with Pipeline Behaviors
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(JobPortal.Review.Application.CompanyReviews.Commands.CreateCompanyReviewCommand).Assembly);

    // Register pipeline behaviors in order
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
});

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(JobPortal.Review.Application.CompanyReviews.Commands.CreateCompanyReviewCommand).Assembly);

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Health Checks
var mongoConnectionString = builder.Configuration.GetSection("MongoDbSettings:ConnectionString").Value ?? "mongodb://localhost:27017";
builder.Services.AddHealthChecks()
    .AddMongoDb(
        mongodbConnectionString: mongoConnectionString,
        name: "mongodb",
        tags: new[] { "database", "mongodb" });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Initialize MongoDB indexes and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Log.Information("Creating MongoDB indexes...");
        var indexService = services.GetRequiredService<IMongoIndexCreationService>();
        await indexService.CreateIndexesAsync();

        Log.Information("Seeding data...");
        var seeder = services.GetRequiredService<IDataSeeder>();
        await seeder.SeedAsync();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while initializing the database");
    }
}

// Configure pipeline
// GlobalExceptionMiddleware is replaced by ExceptionHandlingBehavior in MediatR pipeline
// app.UseMiddleware<GlobalExceptionMiddleware>();

// Use CorrelationId from ServiceDefaults (if running in Aspire)
#if !DEBUG
app.UseCorrelationId();
#endif

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Job Portal Review API v1");
    options.RoutePrefix = string.Empty;
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.MapControllers();

// ============================================
// Map Aspire default endpoints (health checks)
// ============================================
#if !DEBUG
app.MapDefaultEndpoints();
#else
app.MapHealthChecks("/health");
#endif

Log.Information("Starting Job Portal Review API");

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
