namespace ChainOfResponsibility;

// The default chaining behavior can be implemented inside a base handler
// class.
internal abstract class AbstractHandler : IHandler
{
    private IHandler nextHandler = null!;

    public IHandler SetNext(IHandler handler)
    {
        nextHandler = handler;

        // Returning a handler from here will let us link handlers in a
        // convenient way like this:
        // monkey.SetNext(squirrel).SetNext(dog);
        return handler;
    }

    public virtual object Handle(object request)
    {
        if (nextHandler is not null)
        {
            return nextHandler.Handle(request);
        }

        return null;
    }
}