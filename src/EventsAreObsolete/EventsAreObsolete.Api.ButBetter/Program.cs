using EventsAreObsolete.Api.ButBetter;
using MediatR;

var builder = WebApplication.CreateBuilder();

builder.Services.AddMediatR(typeof(Program));
builder.Services.AddTransient<TransientService>();
builder.Services.AddHostedService<TickerBackgroundService>();

var app = builder.Build();

app.Run();