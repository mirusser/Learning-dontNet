using SynchronizationDemo.SimpleExamples;

ProducerConsumerWithExplicitMonitor foo = new();

foo.Run();

Console.WriteLine("Press any key to exit...");
Console.ReadKey(); 