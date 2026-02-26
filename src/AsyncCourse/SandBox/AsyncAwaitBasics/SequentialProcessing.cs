namespace SandBox.AsyncAwaitBasics;

public class SequentialProcessing
{
    public async Task RunAsync()
    {
        var tasks = Enumerable
            .Range(0, 3)
            .Select(i =>
            {
                Console.WriteLine(i);
                return Task.Delay(1000);
            })
            .ToList(); //very important to call ToList() to materialize IEnumerable and start tasks

        foreach (var task in tasks)
        {
            await task;
        }
    }
}