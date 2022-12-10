using System;

namespace CrossPlatformUIDemo.Products;

internal class WindowsButton : IButton
{
    public void OnClick(string param)
    {
        Console.WriteLine($"OnClick: {nameof(WindowsButton)}: {param}");
    }

    public void Render()
    {
        Console.WriteLine($"Rendering: {nameof(WindowsButton)}");
    }
}