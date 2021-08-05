using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorApiClient.DTOs
{
    public class LaunchDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("is_tentative")]
        public bool IsTentative { get; set; }

        [JsonPropertyName("launch_date_local")]
        public DateTimeOffset LaunchDateLocal { get; set; }


        [JsonPropertyName("mission_name")]
        public string MissionName { get; set; }
    }
}
