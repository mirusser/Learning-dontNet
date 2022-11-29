namespace Command;

// However, some commands can delegate more complex operations to other
// objects, called "receivers."
internal class ComplexCommand : ICommand
{
    private Receiver receiver;

    // Context data, required for launching the receiver's methods.
    private string a;

    private string b;

    // Complex commands can accept one or several receiver objects along
    // with any context data via the constructor.
    public ComplexCommand(
        Receiver receiver,
        string a,
        string b)
    {
        this.receiver = receiver;
        this.a = a;
        this.b = b;
    }

    // Commands can delegate to any methods of a receiver.
    public void Execute()
    {
        Console.WriteLine("ComplexCommand: Complex stuff should be done by a receiver object.");
        this.receiver.DoSomething(a);
        this.receiver.DoSomethingElse(b);
    }
}