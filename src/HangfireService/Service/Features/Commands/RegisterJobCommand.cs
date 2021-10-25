using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using HangfireService.Models;
using MediatR;

namespace HangfireService.Features.Commands
{
    public class RegisterJobCommand : IRequest
    {
        public string JobName { get; set; } = null!;
        public string ServiceName { get; set; } = null!;
        public string CronExpression { get; set; } = null!;
        public JobType JobType { get; set; }
        public string? Url { get; set; }
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
            var jobCommand = GetJobCommand(request.JobType, request.JobName, request.ServiceName, request.Url);

            _recurringJobManager.AddOrUpdate(request.JobName, () => _mediator.Send(jobCommand, cancellationToken), request.CronExpression);
            return Task.FromResult(new Unit());
        }

        private IRequest GetJobCommand(JobType jobType, string jobName, string serviceName, string? url)
        {
            switch (jobType)
            {
                case JobType.RunJob:
                    return new RunJobCommand() { JobName = jobName, ServiceName = serviceName };
                case JobType.CallEndpointJob:
                    return new CallEndpointJobCommand() { JobName = jobName, ServiceName = serviceName, Url = url ?? "" };
                default:
                    throw new ArgumentException($"Wrong argument: {nameof(JobType)}, value: {jobType}");
            }
        }
    }
}