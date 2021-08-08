using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorApiClient.DTOs;
using BlazorApiClient.Pages;

namespace BlazorApiClient.DataServices
{
    public class GraphQLSPaceXDataService : ISpaceXDataService
    {
        private readonly HttpClient _httpClient;

        public GraphQLSPaceXDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //TODO: I could use 'Strawberry shake client' here
        public async Task<LaunchDto[]> GetAllLaunches()
        {
            var queryObject = new
            {
                query = @"{
                    launches{
                        id
                        is_tentative
                        mission_name
                        launch_date_local
                    }
                }",

                variables = new { }
            };

            var serializedQueryObject = JsonSerializer.Serialize(queryObject);

            StringContent launchQuery = new(serializedQueryObject, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("graphql", launchQuery);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStreamAsync();
                var gqlData = await JsonSerializer.DeserializeAsync<GqlData>(responseContent);

                return gqlData.Data.Launches;
            }

            return null;
        }
    }
}
