using System.Runtime.CompilerServices;
using ExternalLib;

namespace AddPropertiesToExternalClassDemo;

public class ExternalObjectProperties
{
    public string? Text { get; set; }
}

public static class ExternalObjectExtensions
{
    public static readonly ConditionalWeakTable<SomeExternalClass, ExternalObjectProperties> Data = new();

    public static ExternalObjectProperties GetProperties(
        this SomeExternalClass externalClass)
    {
        return Data.GetOrCreateValue(externalClass);
    }
}