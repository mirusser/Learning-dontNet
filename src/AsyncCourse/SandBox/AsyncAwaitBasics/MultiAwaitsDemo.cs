using System.ComponentModel;
using SandBox.ExtensionMethods;

namespace SandBox.AsyncAwaitBasics;

[Description("Multiple await demo")]
public class MultiAwaitsDemo
{
    public async Task RunAsync()
    {
        Task task = SleepAsync();
        "Before first await".DumpThread();
        await task;
        "Before second await".DumpThread();
        await task;
        "The end".DumpThread();
    }

    private static async Task SleepAsync()
    {
        "SleepAsync started".DumpThread();
        await Task.Delay(1000);
        "SleepAsync ended".DumpThread();
    }
}