using Microsoft.Extensions.Options;

namespace RandomQuotesApi.Middlewares;

// TODO: move it to its own file later
public class ApiKeyOptions
{
    public string HeaderName { get; init; } = "X-Api-Key";
    public string Key { get; init; } = string.Empty;
}

public class ApiKeyMiddleware(IOptions<ApiKeyOptions> options) : IMiddleware
{
    private readonly ApiKeyOptions options = options.Value;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // 1. Try to get the header
        if (!context.Request.Headers.TryGetValue(options.HeaderName, out var providedKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API key missing.");
            return;
        }

        // 2. Compare with configured key
        if (!string.Equals(providedKey, options.Key, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid API key.");
            return;
        }

        // 3. Continue to pipeline
        await next(context);
    }
}