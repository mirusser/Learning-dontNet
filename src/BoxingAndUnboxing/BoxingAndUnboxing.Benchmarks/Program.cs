using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchy>();

[MemoryDiagnoser]
public class Benchy
{
    private static readonly BoxingService BoxingService = new();
    private static readonly object Boxed = 420;

    [Benchmark]
    public object BoxValue()
        => BoxingService.BoxValue(420);

    [Benchmark]
    public int UnboxValue()
        => BoxingService.UnboxValue(Boxed);

    [Benchmark]
    public int SimpleReturn()
        => BoxingService.SimpleReturnInt(420);

    [Benchmark]
    public object SimpleReturnObject()
        => BoxingService.SimpleReturnObject(Boxed);
}

public class BoxingService
{
    public object BoxValue(int number)
    {
        return number;
    }

    public int UnboxValue(object value)
    {
        return (int)value;
    }

    public int SimpleReturnInt(int number)
    {
        return number;
    }

    public object SimpleReturnObject(object value)
    {
        return value;
    }
}