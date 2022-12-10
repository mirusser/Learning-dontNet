using CrossPlatformUIDemo.Products;

namespace CrossPlatformUIDemo.Creators;

public class WebDialog : Dialog
{
    public override IButton CreateButton()
    {
        return new HtmlButton();
    }
}