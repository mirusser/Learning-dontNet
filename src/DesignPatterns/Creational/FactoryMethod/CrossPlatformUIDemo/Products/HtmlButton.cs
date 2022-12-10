namespace CrossPlatformUIDemo.Products;

internal class HtmlButton : IButton
{
    public void OnClick(string param)
    {
        Console.WriteLine($"OnClick: {nameof(HtmlButton)}: {param}");
    }

    public void Render()
    {
        Console.WriteLine($"Rendering: {nameof(HtmlButton)}");
    }
}