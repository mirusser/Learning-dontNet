namespace RefKeywordDemo;

public class StringHolderClass
{
    public string Value { get; set; } = string.Empty;

    public override string ToString() => Value ?? string.Empty;
}

public struct StringHolderStruct
{
    public string Value { get; set; }

    public readonly override string ToString() => Value ?? string.Empty;
}

// ref struct: stack-only type (e.g., Span<T> style). Useful when you need stack semantics.
// Here it's just to mirror the examples above.
public ref struct StringHolderRefStruct
{
    public string Value { get; set; }

    public readonly override string ToString() => Value ?? string.Empty;
}