using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApiClient.DTOs;
using Microsoft.AspNetCore.Components;

namespace BlazorApiClient.Pages
{
    public partial class FetchData
    {
        [Inject]
        private HttpClient Http { get; set; }
        private LaunchDto[] launches;

        protected override async Task OnInitializedAsync()
        {
            launches = await Http.GetFromJsonAsync<LaunchDto[]>("rest/launches");
        }
    }
}
