using System.Threading;
using System.Threading.Tasks;
using HangfireService.Clients;
using MediatR;

namespace HangfireService.Features.Commands
{
    public class CallEndpointJobCommand : IRequest
    {
        public string JobName { get; set; } = null!;
        public string ServiceName { get; set; } = null!;
        public string Url { get; set; } = null!;
    }

    public class CallEndpointJobHandler : IRequestHandler<CallEndpointJobCommand>
    {
        private readonly ICallEndpointClient _callEndpointClient;

        public CallEndpointJobHandler(ICallEndpointClient callEndpointClient)
        {
            _callEndpointClient = callEndpointClient;
        }

        public async Task<Unit> Handle(CallEndpointJobCommand request, CancellationToken cancellationToken)
        {
            await _callEndpointClient.GetMethod(request.Url, cancellationToken).ConfigureAwait(false);

            return new();
        }
    }
}