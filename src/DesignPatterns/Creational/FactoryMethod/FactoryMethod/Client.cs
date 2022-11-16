namespace FactoryMethod;

internal class Client
{
    public void Do()
    {
        Console.WriteLine("App: Launched with the ConcreteCreator1.");
        ClientCode(new ConcreteCreators());

        Console.WriteLine(Environment.NewLine);

        Console.WriteLine("App: Launched with the ConcreteCreator2.");
        ClientCode(new ConcreteCreator2());
    }

    //The client code works with an instance of a concrete creator, albeit
    //through its base interface. As long as the client keeps working with
    //the cretor via the base interface, you can pass it any creator's sublclass
    public void ClientCode(Creator creator)
    {
        // ...
        Console.WriteLine($"Client: I'm not aware of the creator's class, but it still works. {Environment.NewLine}{creator.SomeOperation()}");
        // ...
    }
}