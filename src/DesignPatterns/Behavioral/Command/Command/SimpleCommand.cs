namespace Command;

// Some commands can implement simple operations on their own.
internal class SimpleCommand : ICommand
{
    private readonly string payload = string.Empty;

    public SimpleCommand(string payload)
    {
        this.payload = payload;
    }

    public void Execute()
    {
        Console.WriteLine($"SimpleCommand: See, I can do simple things like printing ({this.payload})");
    }
}