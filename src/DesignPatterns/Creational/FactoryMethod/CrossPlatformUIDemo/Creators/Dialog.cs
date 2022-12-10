using CrossPlatformUIDemo.Products;

namespace CrossPlatformUIDemo.Creators;

public abstract class Dialog
{
    public virtual void Render() 
    {
        IButton okButton = CreateButton();

        okButton.OnClick("Close dialog");
        okButton.Render();
    }

    public abstract IButton CreateButton();
}