using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MinimalAPI_Les.Data;
using MinimalAPI_Les.Dtos;
using MinimalAPI_Les.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConBuilder = new SqlConnectionStringBuilder()
{
    ConnectionString = builder.Configuration.GetConnectionString("SQlDbConnection"),
    UserID = builder.Configuration["UserId"],
    Password = builder.Configuration["Password"],
};

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConBuilder.ConnectionString));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/v1/commands", async (ICommandRepo repo, IMapper mapper) =>
{
    var commands = await repo.GetAllCommandsAsync();
    var commandReadDtos = mapper.Map<IEnumerable<CommandReadDto>>(commands);
    return Results.Ok(commandReadDtos);
});

app.MapGet("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id) =>
{
    var command = await repo.GetCommandByIdAsync(id);

    if (command is not null)
    {
        var commandReadDto = mapper.Map<CommandReadDto>(command);
        return Results.Ok(commandReadDto);
    }

    return Results.NotFound();
});

app.MapPost("api/v1/commands", async(ICommandRepo repo, IMapper mapper, CommandCreateDto dto) => {
    var command = mapper.Map<Command>(dto);
    await repo.CreateCommandAsync(command);
    await repo.SaveChangesAsync();

    var commandReadDto = mapper.Map<CommandReadDto>(command);

    return Results.Created($"api/v1/commands/{commandReadDto.Id}", commandReadDto);
});

app.MapPut(
    "api/v1/commands/{id}", 
    async (ICommandRepo repo, IMapper mapper, int id, CommandUpdateDto dto) =>
{
    var command = await repo.GetCommandByIdAsync(id);

    if (command is not null)
    {
        mapper.Map(dto, command);
        await repo.SaveChangesAsync();

        return Results.NoContent();
    }

    return Results.NotFound();
});

app.MapDelete("api/v1/commands/{id}", async(ICommandRepo repo, IMapper mapper, int id) => {
    var command = await repo.GetCommandByIdAsync(id);

    if (command is not null)
    {
        repo.DeleteCommand(command);
        await repo.SaveChangesAsync();

        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();