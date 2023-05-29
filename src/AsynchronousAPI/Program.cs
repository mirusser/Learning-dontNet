using AsynchronousAPI.Data;
using AsynchronousAPI.Dtos;
using AsynchronousAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddDbContext<AppDbContext>(opt
        => opt.UseSqlite("Data Source=RequestDb.db"));
}

var app = builder.Build();

{
    app.UseHttpsRedirection();

    //endpoints
    {
        // Start endpoint
        app.MapPost(
            "api/v1/products",
            async (AppDbContext context, ListingRequest request) =>
        {
            if (request is null)
            {
                return Results.BadRequest();
            }

            request.RequestStatus = "ACCEPT";
            request.EstimatedCompletionTime = "2023-02-06:14:00:00";

            await context.ListingRequests.AddAsync(request);
            await context.SaveChangesAsync();

            return Results.Accepted(
                $"api/v1/productstatus/{request.RequestId}",
                request);
        });

        // Status endpoint
        app.MapGet(
            "api/v1/productstatus/{requestId}",
            (AppDbContext context, string requestId) =>
            {
                var listingRequest = context.ListingRequests
                    .FirstOrDefault(lr => lr.RequestId == requestId);

                if (listingRequest is null)
                {
                    return Results.NotFound();
                }

                ListingStatus listingStatus = new()
                {
                    RequestStatus = listingRequest.RequestStatus,
                    ResourceUrl = string.Empty
                };

                if (string.Equals(
                    listingRequest.RequestStatus,
                    "COMPLETE",
                    StringComparison.OrdinalIgnoreCase))
                {
                    listingStatus.ResourceUrl = $"api/v1/products/{Guid.NewGuid()}";

                    //return Results.Ok(listingStatus);

                    return Results.Redirect($"https://localhost:7030/{listingStatus.ResourceUrl}");
                }

                listingStatus.EstimatedCompletionTime = "2023-02-06:15:00:00";

                return Results.Ok(listingStatus);
            });

        // Final endpoint
        app.MapGet("api/v1/products/{requestId}", (string requestId) =>
        {
            return Results.Ok("This is where you would pass back the final result");
        });
    }
}

app.Run();