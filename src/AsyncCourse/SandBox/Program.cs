
using SandBox.AsyncAwaitBasics;
using SandBox.TaskAPI;

var foo = new SequentialProcessing();
await foo.RunAsync();

string text = await File.ReadAllTextAsync("D:\\temp.txt");
Console.WriteLine(text);

Console.ReadKey();