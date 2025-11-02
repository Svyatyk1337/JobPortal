using JobPortal.Aggregator.Services;
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
    client.BaseAddress = new Uri("http://application-service");
})
.AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogServiceClient, CatalogServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://catalog-service");
})
.AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

builder.Services.AddHttpClient<IReviewServiceClient, ReviewServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://review-service");
})
.AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

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
