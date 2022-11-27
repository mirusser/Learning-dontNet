## *Simple* Global error handling *with ProblemDetails*

### 1. Middleware approach:
 *Program.cs*
```csharp
    app.UseMiddleware<ErrorHandlingMiddleware>();
```

*ErrorHandlingMiddleware.cs*
```csharp
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(
        HttpContext context,
        Exception ex)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        var result = JsonSerializer.Serialize(new { error = $"An error occured while processing your requst: {ex.Message}" });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        
        return context.Response.WriteAsync(result);
    }
}
```

---

### 2. Filters approach:

*Program.cs*
```csharp
    builder.Services.AddControllers(options =>
        options.Filters.Add<ErrorHandlingFilterAttribute>());
```    
>You can also add filter per controller or action 
(There is no need to register it globally in *Program.cs*)

*ErrorHandlingFilterAttribute.cs*
```csharp
public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is null)
        {
            return;
        }

        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An error occured while processing your request",
            Instance = context.HttpContext.Request.Path,
            Status = (int)HttpStatusCode.InternalServerError,
            Detail = context.Exception.Message
        };

        context.Result = new ObjectResult(problemDetails);

        context.ExceptionHandled = true;
    }
}
```

### 3. Error endpoint
*Program.cs*
```csharp
app.UseExceptionHandler("/error");
```

>From this point you can create endpoint using controller approach or minimal api approach

#### Minimal Api
*Program.cs*
```csharp
    app.Map("/error", (HttpContext httpContext) => 
    {
        Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>().Error;

        return Results.Problem(extensions: new Dictionary<string, object?>() { { "customProperty", "customValue"} });
    });
```

#### Controller
*ErrorsController.cs*
```csharp
public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        //how to access exception from 'HttpContext':
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        //return Problem(statusCode: 400);

        return Problem();
    }
}
```
>with custom Problem Details Factory

*CustomProblemDetailsFactory.cs*
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class CustomProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions options;

    public CustomProblemDetailsFactory(
        IOptions<ApiBehaviorOptions> options)
    {
        this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        statusCode ??= 500;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance,
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        if (modelStateDictionary == null)
        {
            throw new ArgumentNullException(nameof(modelStateDictionary));
        }

        statusCode ??= 400;

        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance,
        };

        if (title != null)
        {
            // For validation problem details, don't overwrite the default title with null.
            problemDetails.Title = title;
        }

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        problemDetails.Extensions.Add("customProperty", "customValue");
    }
}

```
*Program.cs*
```csharp
    builder.Services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();
```