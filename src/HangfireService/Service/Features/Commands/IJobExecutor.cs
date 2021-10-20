using System.Threading.Tasks;

namespace HangfireService.Features.Commands
{
    public interface IJobExecutor
    {
        Task Run(string jobName, string serviceName);
    }
}