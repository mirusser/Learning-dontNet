using Core.Contracts;
using Core.Settings;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddControllers();
    builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection(nameof(TenantSettings)));
    builder.Services.AddTransient<ITenantService, TenantService>();
    builder.Services.AddTransient<IProductService, ProductService>();
    builder.Services.AddAndMigrateTenantDatabases(builder.Configuration);
    builder.Services.AddOpenApi();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();

        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "Developer API v1"); });
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}

app.Run();