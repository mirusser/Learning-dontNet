using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GrpcServer;

namespace GrpcClientFactoryIntegrationDemo.Services
{
    public interface IDemoService
    {
        IAsyncEnumerable<HelloReply> SayHellos();
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}