
using HttpClientDemo.Models;

namespace HttpClientDemo.Clients;

public class OpenWeatherClient : IWeatherClient
{
    private const string openWeatherMapApikey = "62fd2626732eb62fa37d5e5e1eee48a3";

    private readonly HttpClient httpClient;

    public OpenWeatherClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    //private readonly IHttpClientFactory httpClientFactory;

    //public OpenWeatherClient(IHttpClientFactory httpClientFactory)
    //{
    //    this.httpClientFactory = httpClientFactory;
    //}

    public async Task<WeatherResponse?> GetCurrentWeatherForCity(string city)
    {
        //var httpClient = httpClientFactory.CreateClient("weatherapi");
        return
            await httpClient.GetFromJsonAsync<WeatherResponse>(
                $"weather?q={city}&appid={openWeatherMapApikey}");
    }
}