using JobPortal.Application.Api.Middleware;
using JobPortal.Application.Bll.Interfaces;
using JobPortal.Application.Bll.Mappings;
using JobPortal.Application.Bll.Services;
using JobPortal.Application.Dal.Interfaces;
using JobPortal.Application.Dal.UnitOfWork;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// Configure Serilog
// ============================================
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// ============================================
// Add services to the container
// ============================================

// Connection string configuration
var connectionString = builder.Configuration.GetConnectionString("ApplicationDb")
    ?? throw new InvalidOperationException("Connection string 'ApplicationDb' not found");

// Register UnitOfWork and repositories
builder.Services.AddScoped<IUnitOfWork>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<UnitOfWork>>();
    return new UnitOfWork(connectionString, logger);
});

// Register BLL services
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IInterviewService, InterviewService>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add controllers
builder.Services.AddControllers()
    .ConfigureApplyingDefaultControllerConventions();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Job Portal Application API",
        Version = "v1",
        Description = "API for managing job applications, candidates, and interviews",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Job Portal Team",
            Email = "support@jobportal.example.com"
        }
    });

    // Include XML comments if available
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
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
// Configure the HTTP request pipeline
// ============================================

// Use Serilog request logging
app.UseSerilogRequestLogging();

// Global exception handling middleware (ProblemDetails)
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Enable Swagger in all environments for demo purposes
// In production, you might want to restrict this
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Job Portal Application API v1");
    options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
});

// HTTPS redirection
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS
app.UseCors("AllowAll");

// Authorization (if needed in future)
// app.UseAuthorization();

// Map controllers
app.MapControllers();

// ============================================
// Health check endpoint
// ============================================
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow,
    service = "JobPortal.Application.Api"
}))
.WithName("HealthCheck")
.WithTags("Health");

// ============================================
// Run the application
// ============================================
Log.Information("Starting Job Portal Application API");

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
