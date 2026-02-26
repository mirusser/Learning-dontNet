namespace SynchronizationDemo.SimpleExamples;

public class SemaphoreSlimSimpleExample
{
    private readonly SemaphoreSlim semaphore = new(initialCount: 2, maxCount: 2);

    internal async Task RunAsync()
    {
        var tasks = new Task[5];

        for (int i = 0; i < 5; i++)
        {
            int workerId = i + 1;
            tasks[i] = DoWorkAsync(workerId);
        }

        await Task.WhenAll(tasks);
    }

    private async Task DoWorkAsync(int workerId)
    {
        Console.WriteLine($"Worker {workerId} waiting...");

        await semaphore.WaitAsync();
        try
        {
            Console.WriteLine($"Worker {workerId} entered");
            await Task.Delay(2000);
            Console.WriteLine($"Worker {workerId} leaving");
        }
        finally
        {
            semaphore.Release();
        }
    }
}