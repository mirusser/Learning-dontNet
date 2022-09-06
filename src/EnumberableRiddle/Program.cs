
IEnumerable<Task> tasks = Enumerable.Range(0, 2)
    .Select(_ => Task.Run(() => Console.Write("*")));

await Task.WhenAll(tasks);

Console.Write($"{tasks.Count()} stars");

IEnumerable<Task> tasks1 = new[]
{
    Task.Run(() => Console.Write("*")),
    Task.Run(() => Console.Write("*")),
};

await Task.WhenAll(tasks1);

Console.Write($"{tasks1.Count()} stars");