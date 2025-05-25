using Dapper.Application.Interfaces;
using Dapper.Core.Entities;
using Dapper.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddInfrastructure();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dapper.WebApi", Version = "v1" });
    });
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dapper.WebApi v1"));
    }

    app.UseHttpsRedirection();
}

// Group all product endpoints under /api/products
var products = app.MapGroup("/api/products")
    .WithTags("Products"); // optional: groups them in Swagger

products.MapGet("/", async (IUnitOfWork uow) =>
        Results.Ok(await uow.Products.GetAllAsync())
    )
    .WithName("GetAllProducts")
    .Produces<List<Product>>();

products.MapGet("/{id:int}", async (int id, IUnitOfWork uow) =>
    {
        var product = await uow.Products.GetByIdAsync(id);
        return product is not null
            ? Results.Ok(product)
            : Results.NotFound();
    })
    .WithName("GetProductById")
    .Produces<Product>()
    .Produces(StatusCodes.Status404NotFound);

products.MapPost("/", async (Product product, IUnitOfWork uow) =>
    {
        var created = await uow.Products.AddAsync(product);
        return Results.Ok(created);
    })
    .WithName("CreateProduct")
    .Accepts<Product>("application/json")
    .Produces<Product>(StatusCodes.Status201Created);

products.MapPut("/{id:int}", async (int id, Product product, IUnitOfWork uow) =>
    {
        if (id != product.Id)
        {
            return Results.BadRequest("ID in path must match ID in body.");
        }

        var updated = await uow.Products.UpdateAsync(product);
        return updated != 0
            ? Results.Ok(updated)
            : Results.NotFound();
    })
    .WithName("UpdateProduct")
    .Accepts<Product>("application/json")
    .Produces<Product>()
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound);

products.MapDelete("/{id:int}", async (int id, IUnitOfWork uow) =>
    {
        var success = await uow.Products.DeleteAsync(id);
        return success != 0
            ? Results.NoContent()
            : Results.NotFound();
    })
    .WithName("DeleteProduct")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);


app.Run();