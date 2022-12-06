using HttpClientDemo.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IWeatherClient, OpenWeatherClient>();

builder.Services.AddHttpClient<IWeatherClient, OpenWeatherClient>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
});

//builder.Services.AddHttpClient("weatherapi", client =>
//{
//    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
//});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
