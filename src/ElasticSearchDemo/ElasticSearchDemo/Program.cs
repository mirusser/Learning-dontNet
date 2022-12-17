using Domain.Consts;
using Domain;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var settings = new ConnectionSettings() 
        .DefaultIndex(Indexes.Default)
        .DefaultMappingFor<StockData>(i => i.IndexName(Indexes.Aliases.StockDemo))
        .EnableDebugMode()
        .EnableApiVersioningHeader(); //https://github.com/elastic/elasticsearch-net/issues/6154

    return new ElasticClient(settings);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
