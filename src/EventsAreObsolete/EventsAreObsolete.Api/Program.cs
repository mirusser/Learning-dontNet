using EventsAreObsolete.Api;

var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<TickerService>();
builder.Services.AddTransient<TransientService>();
builder.Services.AddHostedService<TickerBackgroundService>();

var app = builder.Build();

app.Run();