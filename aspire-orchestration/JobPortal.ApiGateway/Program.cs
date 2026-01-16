using JobPortal.ApiGateway.Middleware;
using JobPortal.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Use CorrelationId middleware
app.UseCorrelationId();

// Use request logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Map default endpoints (health checks)
app.MapDefaultEndpoints();

// Map reverse proxy
app.MapReverseProxy();

app.Run();
