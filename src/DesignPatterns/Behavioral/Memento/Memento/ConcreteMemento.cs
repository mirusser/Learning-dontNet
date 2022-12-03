namespace Memento;

// The Concrete Memento contains the infrastructure for storing the
// Originator's state.
public class ConcreteMemento : IMemento
{
    private string state;

    private DateTime date;

    public ConcreteMemento(string state)
    {
        this.state = state;
        date = DateTime.Now;
    }

    // The Originator uses this method when restoring its state.
    public string GetState()
        => state;

    // The rest of the methods are used by the Caretaker to display
    // metadata.
    public string GetName()
        => $"{date} / ({state[..9]})...";

    public DateTime GetDate() 
        => date;
}