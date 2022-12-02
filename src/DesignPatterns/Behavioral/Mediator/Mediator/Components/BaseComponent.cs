namespace Mediator.Components;

// The Base Component provides the basic functionality of storing a
// mediator's instance inside component objects.
internal class BaseComponent
{
    protected IMediator mediator;

    public BaseComponent(IMediator mediator = null)
    {
        this.mediator = mediator;
    }

    public void SetMediator(IMediator mediator)
    {
        this.mediator = mediator;
    }
}