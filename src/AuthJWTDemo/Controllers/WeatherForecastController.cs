using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AuthJWTDemo.Controllers;

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly JWTAuthenticationManager _authenticationManager;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        JWTAuthenticationManager authenticationManager)
    {
        _logger = logger;
        _authenticationManager = authenticationManager;
    }

    [Authorize]
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [AllowAnonymous]
    [HttpPost(Name = "Authorize")]
    public IResult AuthUser([FromBody] User user)
    {
        var token = _authenticationManager.Authenticate(user.Username, user.Password);
        if (token is null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(token);
    }
}

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
}
