using Microsoft.OpenApi.Models;
using MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container (old: 'ConfigureServices' method)

builder.Services.AddHealthChecks();
builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
    options.Delay = TimeSpan.FromSeconds(2);
    options.Predicate = (check) => check.Tags.Contains("ready");
});

builder.Services.AddMassTransit(x => x.UsingRabbitMq());
builder.Services.AddMassTransitHostedService(waitUntilStarted: true);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Publisher", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Publisher v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

//app.MapControllers();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
    {
        Predicate = (check) => check.Tags.Contains("ready"),
    });

    endpoints.MapHealthChecks("/health/live", new HealthCheckOptions());
});

app.Run();
