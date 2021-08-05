using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApiClient.DTOs;
using BlazorApiClient.Pages;
using static System.Net.WebRequestMethods;

namespace BlazorApiClient.DataServices
{
    public class RestSpaceXDataService : ISpaceXDataService
    {
        private readonly HttpClient _httpClient;

        public RestSpaceXDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<LaunchDto[]> GetAllLaunches()
        {
            var launches = 
                await _httpClient.GetFromJsonAsync<LaunchDto[]>("rest/launches");

            return launches;
        }
    }
}
