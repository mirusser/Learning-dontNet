using HttpClientDemo.Models;

namespace HttpClientDemo.Clients;

public interface IWeatherClient
{
    Task<WeatherResponse?> GetCurrentWeatherForCity(string city);
}