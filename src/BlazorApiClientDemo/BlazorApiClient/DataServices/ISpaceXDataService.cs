using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApiClient.DTOs;

namespace BlazorApiClient.DataServices
{
    public interface ISpaceXDataService
    {
        Task<LaunchDto[]> GetAllLaunches();
    }
}
