namespace SynchronizationDemo.SimpleExamples;

public class SemaphoreSimpleExample
{
    private readonly Semaphore semaphore = new Semaphore(initialCount: 2, maximumCount: 2);

    // At most 2 threads at a time can be inside.
    internal void Run()
    {
        for (int i = 1; i <= 5; i++)
        {
            int workerId = i;
            new Thread(() => DoWork(workerId)).Start();
        }
    }

    private void DoWork(int workerId)
    {
        Console.WriteLine($"Worker {workerId} is waiting...");

        semaphore.WaitOne(); // Take one slot
        try
        {
            Console.WriteLine($"Worker {workerId} entered");
            Thread.Sleep(2000); // Simulate work
            Console.WriteLine($"Worker {workerId} leaving");
        }
        finally
        {
            semaphore.Release(); // Return one slot
        }
    }
    
}