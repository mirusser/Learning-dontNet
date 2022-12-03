namespace Memento;

// The Caretaker doesn't depend on the Concrete Memento class. Therefore, it
// doesn't have access to the originator's state, stored inside the memento.
// It works with all mementos via the base Memento interface.
public class Caretaker
{
    private readonly List<IMemento> mementos = new();

    private readonly Originator originator;

    public Caretaker(Originator originator)
    {
        this.originator = originator;
    }

    public void Backup()
    {
        Console.WriteLine("\nCaretaker: Saving Originator's state...");
        mementos.Add(originator.Save());
    }

    public void Undo()
    {
        if (mementos.Count == 0)
        {
            return;
        }

        var memento = mementos.Last();
        mementos.Remove(memento);

        Console.WriteLine("Caretaker: Restoring state to: " + memento.GetName());

        try
        {
            originator.Restore(memento);
        }
        catch (Exception)
        {
            Undo();
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine("Caretaker: Here's the list of mementos:");

        foreach (var memento in mementos)
        {
            Console.WriteLine(memento.GetName());
        }
    }
}