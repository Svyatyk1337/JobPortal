using Grpc.Net.ClientFactory;
using JobPortal.Aggregator.Services;
using JobPortal.Catalog.WebApi.Protos;
using JobPortal.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HttpContextAccessor for CorrelationId
builder.Services.AddHttpContextAccessor();

// Register typed HTTP clients for microservices
builder.Services.AddHttpClient<IApplicationServiceClient, ApplicationServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://application-service:8080");
})
.AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogServiceClient, CatalogServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://catalog-service:8080");
})
.AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

builder.Services.AddHttpClient<IReviewServiceClient, ReviewServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://review-service:8080");
})
.AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

// Register gRPC client for Catalog Service
builder.Services.AddGrpcClient<CatalogService.CatalogServiceClient>("catalog-service", options =>
{
    // In Docker, gRPC runs on port 8081 (HTTP/2), REST API on 8080 (HTTP/1.1)
    var catalogGrpcUrl = builder.Configuration["Services:CatalogService:GrpcUrl"] ?? "http://catalog-service:8081";
    options.Address = new Uri(catalogGrpcUrl);
});

builder.Services.AddScoped<ICatalogGrpcClient, CatalogGrpcClient>();

// Register CorrelationIdDelegatingHandler
builder.Services.AddTransient<CorrelationIdDelegatingHandler>();

var app = builder.Build();

app.UseCorrelationId();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();
