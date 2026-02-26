namespace SandBox.TaskAPI;

public class TaskExceptionHandling
{
    public void Execute()
    {
        var task = Task.Run(() =>
        {
            Console.WriteLine("Running...");
            throw new NullReferenceException();
            //return 0; //unreachable code
        });
        Console.ReadKey(); // Nothing happens here, an unhandled exception was stored ("recorded") in a Task.

        try
        {
            //We must "observe" a task via Wait or Result to observe the exception
            //var result = task.Result;
        }
        catch (AggregateException ex) when (ex.InnerException is NullReferenceException)
        {
            Console.WriteLine($"Handled exception: {Environment.NewLine} {ex}");
        }
    }
}