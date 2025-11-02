using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace JobPortal.ServiceDefaults;

public class CorrelationIdDelegatingHandler : DelegatingHandler
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var correlationId = _httpContextAccessor.HttpContext?.Request.Headers[CorrelationIdHeaderName].ToString();

        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            request.Headers.TryAddWithoutValidation(CorrelationIdHeaderName, correlationId);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
