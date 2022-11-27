namespace ChainOfResponsibility.ConcreteHandlers;

internal class DogHandler : AbstractHandler
{
    public override object Handle(object request)
    {
        if (request.ToString() == "MeatBall")
        {
            return $"Dog: I'll eat the {request}.{Environment.NewLine}";
        }
        else
        {
            return base.Handle(request);
        }
    }
}