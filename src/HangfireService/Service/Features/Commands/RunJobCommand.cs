using System.Threading;
using System.Threading.Tasks;
using Jobs;
using MassTransit;
using MediatR;

namespace HangfireService.Features.Commands
{
    public class RunJobCommand : IRequest
    {
        public string JobName { get; set; } = null!;
        public string ServiceName { get; set; } = null!;
    }

    public class RunJobHandler : IRequestHandler<RunJobCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RunJobHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task<Unit> Handle(RunJobCommand request, CancellationToken cancellationToken)
        {
            Task.FromResult(_publishEndpoint.Publish<IJobMessage>(new { JobName = request.JobName, ServiceName = request.JobName }));

            return Task.FromResult(new Unit());
        }
    }
}