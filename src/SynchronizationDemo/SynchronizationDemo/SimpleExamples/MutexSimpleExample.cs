namespace SynchronizationDemo.SimpleExamples;

public class MutexSimpleExample
{
    private readonly Mutex mutex = new();

    // 3 threads start, only one thread at a time enters the critical section.
    internal void Run()
    {
        for (int i = 1; i <= 3; i++)
        {
            int workerId = i;
            new Thread(() => DoWork(workerId)).Start();
        }
    }

    private void DoWork(int workerId)
    {
        Console.WriteLine($"Worker {workerId} is waiting...");

        mutex.WaitOne(); // Acquire the mutex
        try
        {
            Console.WriteLine($"Worker {workerId} entered critical section");
            Thread.Sleep(1000); // Simulate work
            Console.WriteLine($"Worker {workerId} leaving critical section");
        }
        finally
        {
            mutex.ReleaseMutex(); // Always release
        }
    }
}