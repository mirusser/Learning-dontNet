using HttpClientDemo.Clients;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherClient weatherClient;

    public WeatherForecastController(IWeatherClient weatherClient)
    {
        this.weatherClient = weatherClient;
    }

    [HttpGet("weather/{city}")]
    public async Task<IActionResult> Forecast(string city)
    {
        var weather = await weatherClient.GetCurrentWeatherForCity(city);

        return weather is not null ? Ok(weather) : NotFound();
    }
}