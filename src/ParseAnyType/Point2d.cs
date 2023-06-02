using System.Diagnostics.CodeAnalysis;

namespace ParseAnyType;

public record Point2d(int X, int Y) : ISpanParsable<Point2d>
{
    public static Point2d Parse(
        string s,
        IFormatProvider? provider = null)
    {
        // example input: "6,9"
        var splitText = s.Split(',');
        var x = splitText[0].Parse<int>();
        var y = splitText[1].Parse<int>();

        return new(x, y);
    }

    public static Point2d Parse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider = null)
    {
        Span<Range> dest = stackalloc Range[2];
        s.Split(dest, ',');
        var x = int.Parse(s[dest[0]]);
        var y = int.Parse(s[dest[1]]);

        return new(x, y);
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Point2d result)
    {
        try
        {
            var splitText = s!.Split(",");
            var x = splitText[0].Parse<int>();
            var y = splitText[1].Parse<int>();
            result = new(x, y);

            return true;
        }
        catch
        {
            result = new(0, 0);

            return false;
        }
    }

    public static bool TryParse(
        ReadOnlySpan<char> s,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out Point2d result)
    {
        try
        {
            Span<Range> dest = stackalloc Range[2];
            var splitInt = s.Split(dest, ',');

            if (splitInt != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(s), "Only two values can be splited");
            }

            var x = int.Parse(s[dest[0]]);
            var y = int.Parse(s[dest[1]]);
            result = new(x, y);

            return true;
        }
        catch
        {
            result = new(0, 0);

            return false;
        }
    }
}