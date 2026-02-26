using Microsoft.EntityFrameworkCore;
using RandomQuotesApi.Data;
using RandomQuotesApi.Middlewares;
using RandomQuotesApi.Repos;
using RandomQuotesApi.Services;
using RandomQuotesApi.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using RandomQuotesApi.Models.Auth;
using RandomQuotesApi.Models.DTOs;
using Microsoft.Extensions.Resilience;
using System.Net;
using RandomQuotesApi.Clients;
using RandomQuotesApi.GraphQL;
using Path = System.IO.Path;

var builder = WebApplication.CreateBuilder(args);
{
    // builder.Services.Configure<ApiKeyOptions>(
    //     builder.Configuration.GetSection("ApiKeyAuth"));
    //
    // builder.Services.AddTransient<ApiKeyMiddleware>();

    builder.Services.AddOptions<JwtOptions>()
        .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
        .Validate(o => !string.IsNullOrWhiteSpace(o.SigningKey), "SigningKey is required")
        .Validate(o => !string.IsNullOrWhiteSpace(o.Issuer), "Issuer is required")
        .Validate(o => !string.IsNullOrWhiteSpace(o.Audience), "Audience is required")
        .ValidateOnStart();

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var jwtOptions = builder.Configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>()!;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(jwtOptions.ExpMinutes)
            };
        });

    builder.Services.AddAuthorization(options => { options.AddPermissionPolicies(); });

    // Connection string from appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                           ?? $"Data Source={Path.Combine(AppContext.BaseDirectory, "app.db")}";

    // EF Core + SQLite
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(connectionString));

    // Caching
    builder.Services.AddDistributedMemoryCache(); // in-memory, IDistributedCache
    builder.Services.AddSingleton<ICacheService, CacheService>();

    // Repositories (data layer)
    builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
    builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ISeenRepository, SeenRepository>();

    // Services
    builder.Services.AddSingleton<IJwtService, JwtService>();
    builder.Services.AddScoped<QuoteService>();
    builder.Services.AddScoped<IFavoritesService, FavoritesService>();

    builder.Services.AddTransient<GlobalExceptionMiddleware>();
    builder.Services.AddControllers();

    builder.Services.AddHttpClient();
    // .ConfigureHttpClientDefaults(http =>
    // {
    //     // Standard pipeline = rate limiter + timeout + retry + circuit breaker + attempt timeout
    //     // applies to all http client instances
    //     http.AddStandardResilienceHandler(options =>
    //     {
    //         // Optional fine-tuning
    //         options.Retry.MaxRetryAttempts = 3;
    //         options.Retry.Delay = TimeSpan.FromSeconds(2);
    //         options.Retry.BackoffType = DelayBackoffType.Exponential;
    //         options.Retry.UseJitter = true;
    //
    //         options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(5);
    //     });
    // });

    // Polly v8-style resilience pipeline for HTTP responses
    builder.Services.AddResiliencePipeline("resilience-pipeline", pipelineBuilder =>
    {
        // Retry with exponential backoff + jitter
        pipelineBuilder.AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(2),
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            // more potential configuration here
        });

        // Add a simple timeout around the whole HTTP call
        pipelineBuilder.AddTimeout(TimeSpan.FromSeconds(5));
    });
    
    builder.Services
        .AddGraphQLServer()
        .AddQueryType<Query>()          // we'll create this
        .AddProjections()
        .AddFiltering()
        .AddSorting()
        .ModifyPagingOptions(opt =>
        {
            opt.DefaultPageSize = 3;
            opt.MaxPageSize = 10;
            opt.IncludeTotalCount = true;
        });

    // Your typed client
    builder.Services.AddScoped<IQuotableClient, QuotableClient>();

    // Optional but useful: Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    // Apply migrations + seed data
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var db = services.GetRequiredService<AppDbContext>();
        var env = services.GetRequiredService<IWebHostEnvironment>();

        await db.Database.MigrateAsync();
        await DataSeeder.SeedPermissionsAsync(db, env);
        await DataSeeder.SeedQuotesAsync(db, env);
    }

    // Swagger UI in dev
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    //app.UseMiddleware<ApiKeyMiddleware>();
    app.UseMiddleware<GlobalExceptionMiddleware>();

    // app.UseHttpsRedirection();

    app.MapControllers();
    app.MapGraphQL(); 
}

app.Run();