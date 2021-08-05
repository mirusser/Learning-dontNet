using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApiClient.DataServices;
using BlazorApiClient.DTOs;
using Microsoft.AspNetCore.Components;

namespace BlazorApiClient.Pages
{
    public partial class Launches
    {
        [Inject]
        private ISpaceXDataService spaceXDataService { get; set; }
        private LaunchDto[] launches;

        protected override async Task OnInitializedAsync()
        {
            launches = await spaceXDataService.GetAllLaunches();
        }
    }
}
