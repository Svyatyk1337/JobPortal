using JobPortal.Review.Application.Common.Mappings;
using JobPortal.Review.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire ServiceDefaults (if available)
try
{
    builder.AddServiceDefaults();
}
catch
{
    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
    });
}

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Job Portal Review API",
        Version = "v1",
        Description = "API for managing reviews using CQRS, MediatR, and MongoDB"
    });
});

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(JobPortal.Review.Application.CompanyReviews.Commands.CreateCompanyReviewCommand).Assembly);
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure pipeline
try { app.MapDefaultEndpoints(); } catch { }

app.UseSerilogRequestLogging();

try { app.UseCorrelationId(); } catch { }

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

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow,
    service = "JobPortal.Review.WebApi"
})).WithTags("Health");

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
