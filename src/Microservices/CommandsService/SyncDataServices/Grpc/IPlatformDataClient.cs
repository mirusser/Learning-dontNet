using System.Collections.Generic;
using System.Threading.Tasks;
using CommandsService.Models;

namespace CommandsService.SyncDataServices.Grpc
{
    public interface IPlatformDataClient
    {
        Task<IEnumerable<Platform>> GetAllPlatforms();
    }
}