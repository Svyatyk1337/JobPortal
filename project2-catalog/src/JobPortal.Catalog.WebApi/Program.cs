using FluentValidation;
using JobPortal.Catalog.Bll.Mappings;
using JobPortal.Catalog.Bll.Services;
using JobPortal.Catalog.Bll.Validators;
using JobPortal.Catalog.Data;
using JobPortal.Catalog.Data.UnitOfWork;
using JobPortal.Catalog.WebApi.GrpcServices;
using JobPortal.Catalog.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

#if !DEBUG
using JobPortal.ServiceDefaults;
#endif

var builder = WebApplication.CreateBuilder(args);

// ============================================
// Configure Kestrel with separate ports for HTTP/1.1 and HTTP/2
// ============================================
builder.WebHost.ConfigureKestrel(options =>
{
    // Port 8080 for REST API (HTTP/1.1)
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    // Port 8081 for gRPC (HTTP/2 without TLS)
    options.ListenAnyIP(8081, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

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

// ============================================
// Add services to the container
// ============================================

// Connection string configuration
var connectionString = builder.Configuration.GetConnectionString("CatalogDb")
    ?? throw new InvalidOperationException("Connection string 'CatalogDb' not found");

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Redis distributed cache (Aspire will auto-inject connection string as "cache")
builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisConnection = builder.Configuration.GetConnectionString("cache");
    if (!string.IsNullOrEmpty(redisConnection))
    {
        options.Configuration = redisConnection;
    }
    else
    {
        // Fallback for local development without Redis
        options.Configuration = "localhost:6379";
    }
});

// Register BLL services
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IJobService, JobService>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateCompanyDtoValidator>();

// Add controllers
builder.Services.AddControllers();

// Add gRPC
builder.Services.AddGrpc();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Job Portal Catalog API",
        Version = "v1",
        Description = "API for managing companies, jobs, and related catalog data using EF Core",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Job Portal Team",
            Email = "support@jobportal.example.com"
        }
    });
});

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ============================================
// Build the application
// ============================================
var app = builder.Build();

// ============================================
// Run migrations and seed data
// ============================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        Log.Information("Database migrated successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while migrating the database");
    }
}

// ============================================
// Configure the HTTP request pipeline
// ============================================

// Use Serilog request logging
app.UseSerilogRequestLogging();

// Use CorrelationId from ServiceDefaults (if running in Aspire)
#if !DEBUG
app.UseCorrelationId();
#endif

// Global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Enable Swagger in all environments for demo purposes
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Job Portal Catalog API v1");
    options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
});

// HTTPS redirection
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS
app.UseCors("AllowAll");

// Map controllers
app.MapControllers();

// Map gRPC services
app.MapGrpcService<CatalogGrpcService>();

// ============================================
// Map Aspire default endpoints (health checks)
// ============================================
#if !DEBUG
app.MapDefaultEndpoints();
#else
// Health check endpoint for local development
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow,
    service = "JobPortal.Catalog.WebApi"
}))
.WithName("HealthCheck")
.WithTags("Health");
#endif

// ============================================
// Run the application
// ============================================
Log.Information("Starting Job Portal Catalog API");

try
{
    app.Run();
    Log.Information("Application stopped gracefully");
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
