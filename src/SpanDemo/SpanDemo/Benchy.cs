using BenchmarkDotNet.Attributes;

namespace SpanDemo;

[MemoryDiagnoser]
public class Benchy
{
    [Params("08 07 2021", "31 12 1999", "01 01 2000")]
    private string Input { get; set; } = null!;

    [Benchmark]
    public (int day, int month, int year) DateWithStringAndSubstring()
    {
        var dayAsText = Input[..2];
        var monthAsText = Input.Substring(3, 2);
        var yearAsText = Input[6..];

        var day = int.Parse(dayAsText);
        var month = int.Parse(monthAsText);
        var year = int.Parse(yearAsText);

        return (day, month, year);
    }

    [Benchmark]
    public (int day, int month, int year) ReadOnlySpan_TryParse()
    {
        ReadOnlySpan<char> s = Input;
        var ok = int.TryParse(s[..2], out var day)
                 & int.TryParse(s.Slice(3, 2), out var month)
                 & int.TryParse(s[6..], out var year);
        
        return !ok ? default : (day, month, year);
    }

}