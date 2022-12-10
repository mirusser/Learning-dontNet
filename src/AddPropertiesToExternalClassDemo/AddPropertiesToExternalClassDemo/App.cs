using ExternalLib;

namespace AddPropertiesToExternalClassDemo;

public class App
{
    public List<SomeExternalClass> Objects { get; } = new();

    public void Run()
    {
        var eo1 = new SomeExternalClass();
        var eo2 = new SomeExternalClass();
        var eo3 = new SomeExternalClass();

        Objects.Add(eo1);
        Objects.Add(eo2);
        Objects.Add(eo3);

        ExternalObjectExtensions.Data.Add(eo1, new ExternalObjectProperties());
        ExternalObjectExtensions.Data.Add(eo2, new ExternalObjectProperties());
        ExternalObjectExtensions.Data.Add(eo3, new ExternalObjectProperties());
    }

    public void Clean()
    {
        Objects.RemoveAt(0);
    }
}