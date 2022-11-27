namespace ChainOfResponsibility.ConcreteHandlers;

internal class MonkeyHandler : AbstractHandler
{
    public override object Handle(object request)
    {
        if ((request as string) == "Banana")
        {
            return $"Monkey: I'll eat the {request}.{Environment.NewLine}";
        }
        else
        {
            return base.Handle(request);
        }
    }
}