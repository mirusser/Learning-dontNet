using System.Threading.Tasks;
using Jobs;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Listener1.Listeners
{
    public class JobListener : IConsumer<IJobMessage>
    {
        private readonly ILogger<JobListener> _logger;

        public JobListener(ILogger<JobListener> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IJobMessage> context)
        {
            if (context?.Message != null)
            {
                //check using 'ServiceName' and 'JobName' if its your job to execute then execute said job, if not just do nothing

                _logger.LogInformation("Listener1: Received event {EventName}, job name: {JobName}, service name: {ServiceName}", nameof(IJobMessage), context.Message.JobName, context.Message.ServiceName);
            }
            else
            {
                _logger.LogWarning("Listener1: Received event {EventName} is null.", nameof(IJobMessage));
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}