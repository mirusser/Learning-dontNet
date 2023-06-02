public static class ParsableExtensions
{
    public static T Parse<T>(
        this string input,
        IFormatProvider? formatProvider = null)
        where T : IParsable<T>
            => T.Parse(input, formatProvider);

    public static T Parse<T>(
        this ReadOnlySpan<char> input,
        IFormatProvider? formatProvider = null)
        where T : ISpanParsable<T>
            => T.Parse(input, formatProvider);

    public static bool TryParse<T>(
        this string input,
        out T? result,
        IFormatProvider? formatProvider = null)
        where T : IParsable<T>
            => T.TryParse(input, formatProvider, out result);
}