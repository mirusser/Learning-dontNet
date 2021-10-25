using System.Threading;
using System.Threading.Tasks;

namespace HangfireService.Clients
{
    public interface ICallEndpointClient
    {
        Task GetMethod(string url, CancellationToken cancellation = default);
    }
}