using System.Net;
using System.Text.Json;
using JobPortal.Application.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Application.Api.Middleware;

/// <summary>
/// Global exception handling middleware
/// Converts domain exceptions to ProblemDetails (RFC 7807 / RFC 9457)
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        var (statusCode, problemDetails) = exception switch
        {
            NotFoundException notFoundEx => (
                HttpStatusCode.NotFound,
                CreateProblemDetails(
                    "Not Found",
                    (int)HttpStatusCode.NotFound,
                    notFoundEx.Message,
                    context.Request.Path)
            ),
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                CreateValidationProblemDetails(
                    "Validation Error",
                    (int)HttpStatusCode.BadRequest,
                    validationEx.Message,
                    context.Request.Path,
                    validationEx.Errors)
            ),
            BusinessConflictException conflictEx => (
                HttpStatusCode.Conflict,
                CreateProblemDetails(
                    "Business Conflict",
                    (int)HttpStatusCode.Conflict,
                    conflictEx.Message,
                    context.Request.Path)
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                CreateProblemDetails(
                    "Internal Server Error",
                    (int)HttpStatusCode.InternalServerError,
                    "An unexpected error occurred",
                    context.Request.Path)
            )
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsJsonAsync(problemDetails, options);
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        string detail,
        string instance)
    {
        return new ProblemDetails
        {
            Title = title,
            Status = status,
            Detail = detail,
            Instance = instance,
            Type = $"https://httpstatuses.com/{status}"
        };
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
        string title,
        int status,
        string detail,
        string instance,
        IDictionary<string, string[]> errors)
    {
        return new ValidationProblemDetails(errors)
        {
            Title = title,
            Status = status,
            Detail = detail,
            Instance = instance,
            Type = $"https://httpstatuses.com/{status}"
        };
    }
}
