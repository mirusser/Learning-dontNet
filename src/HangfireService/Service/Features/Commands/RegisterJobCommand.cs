using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;

namespace HangfireService.Features.Commands
{
    public class RegisterJobCommand : IRequest
    {
        public string JobName { get; set; } = null!;
        public string ServiceName { get; set; } = null!;
        public string CronExpression { get; set; } = null!;
    }

    public class RegisterJobHandler : IRequestHandler<RegisterJobCommand>
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IMediator _mediator;

        public RegisterJobHandler(IRecurringJobManager recurringJobManager, IMediator mediator)
        {
            _recurringJobManager = recurringJobManager;
            _mediator = mediator;
        }

        public Task<Unit> Handle(RegisterJobCommand request, CancellationToken cancellationToken)
        {
            var runJobCommand = new RunJobCommand() { JobName = request.JobName, ServiceName = request.ServiceName };

            _recurringJobManager.AddOrUpdate(request.JobName, () => _mediator.Send(runJobCommand, cancellationToken), request.CronExpression);
            return Task.FromResult(new Unit());
        }
    }
}