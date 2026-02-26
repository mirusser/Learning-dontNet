
using SandBox.AsyncAwaitBasics;
using SandBox.TaskAPI;

var foo = new SequentialProcessing();
await foo.RunAsync();

Console.ReadKey();