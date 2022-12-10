using CrossPlatformUIDemo.Products;

namespace CrossPlatformUIDemo.Creators;

public class WindowsDialog : Dialog
{
    public override IButton CreateButton()
    {
        return new WindowsButton();
    }
}