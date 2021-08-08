using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlazorApiClient.Pages;

namespace BlazorApiClient.DTOs
{
    public partial class GqlData
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("launches")]
        public LaunchDto[] Launches { get; set; }
    }
}
