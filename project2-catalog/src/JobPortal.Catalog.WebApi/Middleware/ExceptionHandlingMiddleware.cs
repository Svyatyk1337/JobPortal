using FluentValidation;
using JobPortal.Catalog.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace JobPortal.Catalog.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var problemDetails = new
        {
            type = "https://tools.ietf.org/html/rfc7231",
            title = "An error occurred",
            status = (int)HttpStatusCode.InternalServerError,
            detail = exception.Message,
            instance = context.Request.Path
        };

        switch (exception)
        {
            case NotFoundException notFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                problemDetails = new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    title = "Resource not found",
                    status = (int)HttpStatusCode.NotFound,
                    detail = notFoundException.Message,
                    instance = context.Request.Path
                };
                break;

            case FluentValidation.ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    title = "Validation error",
                    status = (int)HttpStatusCode.BadRequest,
                    detail = "One or more validation errors occurred",
                    instance = context.Request.Path,
                    errors = validationException.Errors.Select(e => new
                    {
                        property = e.PropertyName,
                        error = e.ErrorMessage
                    })
                };
                break;

            case DomainException domainException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new
                {
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    title = "Business rule violation",
                    status = (int)HttpStatusCode.BadRequest,
                    detail = domainException.Message,
                    instance = context.Request.Path
                };
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var result = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(result);
    }
}
