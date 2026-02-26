using Microsoft.AspNetCore.Mvc;

namespace RandomQuotesApi.Middlewares;

public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = "Something went wrong while processing your request.",
                Type = "https://your-api/errors/internal-server-error",
                Extensions =
                {
                    ["traceId"] = context.TraceIdentifier
                }
            };

            context.Response.StatusCode = problem.Status!.Value;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
