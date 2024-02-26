using GrpcToRestDemo.Data;
using GrpcToRestDemo.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=ToDoDatabase.db"));
    builder.Services
        .AddGrpc()
        .AddJsonTranscoding();
    builder.Services.AddGrpcReflection();
}

var app = builder.Build();

{
    app.MapGrpcService<GreeterService>();
    app.MapGrpcService<ToDoService>();
    app.MapGet(
            "/",
            () => "Communication with gRPC endpoints must be made through a gRPC client.");

    IWebHostEnvironment env = app.Environment;
    if (env.IsDevelopment())
    {
        app.MapGrpcReflectionService();
    }
}

app.Run();