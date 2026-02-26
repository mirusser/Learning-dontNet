using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchy>();

[MemoryDiagnoser()]
public class Benchy
{
    private const string ClearValue = "Password123!";
    
    // [Params("Password123!", "HelloWorld", "ABC123456")]
    // public string ClearValue { get; set; } = null!;
    
    [Benchmark]
    public string MaskNaive()
    {
        var firstChars = ClearValue[..3];
        var length = ClearValue.Length - firstChars.Length;

        for (var i = 0; i < length; i++)
        {
            firstChars += "*";
        }

        return firstChars;
    }

    [Benchmark]
    public string MaskStringBuilder()
    {
        var firstChars = ClearValue[..3];
        var length = ClearValue.Length - firstChars.Length;
        var stringBuilder = new StringBuilder(firstChars);

        for (var i = 0; i < length; i++)
        {
            stringBuilder.Append('*');
        }

        return stringBuilder.ToString();
    }
    
    [Benchmark]
    public string MaskNewString()
    {
        var firstChars = ClearValue[..3];
        var length = ClearValue.Length - firstChars.Length;
        var asterisks = new string('*', length);
        
        return firstChars + asterisks;
    }

    [Benchmark]
    public string MaskStringCreate()
    {
        return string.Create(ClearValue.Length, ClearValue, (span, value) =>
        {
            value.AsSpan().CopyTo(span);
            span[3..].Fill('*');
        });
    }
}