namespace SandBox.TaskAPI;

public class ContinueWith
{
    public void Execute()
    {
        Task<int> task = Task.Run(() =>
        {
            Console.WriteLine("Running t1");
            return 1;
        });
        Task task1 = task.ContinueWith(t =>
        {
            Console.WriteLine($"Running t2 after t1 with result: {t.Result}");
        });

        task1.Wait();
    }

    public void ExecuteWithExceptionHandling()
    {
        Task<int> task1 = Task.Run(() =>
        {
            Console.WriteLine("Running t1");
            throw new NullReferenceException();
            return 1;
        });

        Task task2 = task1.ContinueWith(t =>
        {
            Console.WriteLine($"Running t2 after t1 with result {t.Result}");
        });

        try
        {
            task2.Wait();
        }
        catch (AggregateException ex) when
            (ex.InnerException is AggregateException
             && ex.InnerException.InnerException is NullReferenceException)
        {
            Console.WriteLine($"Running t2.Wait results in an exception {ex}");
        }
    }
}