using Singleton;

NaiveSingleton s1 = NaiveSingleton.GetInstance();
NaiveSingleton s2 = NaiveSingleton.GetInstance();

if (s1 == s2)
{
    Console.WriteLine("Naive Singleton works, both variables contain the same instance.");
}
else
{
    Console.WriteLine("Naive Singletonfailed, variables contain different instances.");
}

Console.ReadLine();

Console.WriteLine(
        "{0}\n{1}\n\n{2}\n",
        "If you see the same value, then singleton was reused (yay!)",
        "If you see different values, then 2 singletons were created (booo!!)",
        "RESULT:"
    );

Thread process1 = new(() => TestThreadSafeSingleton("FOO"));
Thread process2 = new(() => TestThreadSafeSingleton("BAR"));

process1.Start();
process2.Start();

process1.Join();
process2.Join();

Console.ReadLine();

static void TestThreadSafeSingleton(string value)
{
    ThreadSafeSingleton singleton = ThreadSafeSingleton.GetInstance(value);
    Console.WriteLine(singleton.Value);
}
