namespace Command;

// The Invoker is associated with one or several commands. It sends a
// request to the command.
public class Invoker
{
    private ICommand onStart;

    private ICommand onFinish;

    // Initialize commands.
    public void SetOnStart(ICommand command)
    {
        onStart = command;
    }

    public void SetOnFinish(ICommand command)
    {
        onFinish = command;
    }

    // The Invoker does not depend on concrete command or receiver classes.
    // The Invoker passes a request to a receiver indirectly, by executing a
    // command.
    public void DoSomethingImportant()
    {
        Console.WriteLine("Invoker: Does anybody want something done before I begin?");
        if (onStart is not null and ICommand)
        {
            onStart.Execute();
        }

        Console.WriteLine("Invoker: ...doing something really important...");

        Console.WriteLine("Invoker: Does anybody want something done after I finish?");
        if (onFinish is not null and ICommand)
        {
            onFinish.Execute();
        }
    }
}