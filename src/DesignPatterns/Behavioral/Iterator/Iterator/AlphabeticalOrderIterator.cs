namespace Iterator;

// Concrete Iterators implement various traversal algorithms. These classes
// store the current traversal position at all times.
public class AlphabeticalOrderIterator : Iterator
{
    private readonly WordsCollection collection;

    // Stores the current traversal position. An iterator may have a lot of
    // other fields for storing iteration state, especially when it is
    // supposed to work with a particular kind of collection.
    private int position = -1;

    private readonly bool reverse;

    public AlphabeticalOrderIterator(
        WordsCollection collection,
        bool reverse = false)
    {
        this.collection = collection;
        this.reverse = reverse;

        if (reverse)
        {
            position = collection.GetItems().Count;
        }
    }

    public override object Current()
        => collection.GetItems()[position];

    public override int Key()
        => position;

    public override bool MoveNext()
    {
        int updatedPosition = position + (reverse ? -1 : 1);

        if (updatedPosition >= 0
            && updatedPosition < collection.GetItems().Count)
        {
            position = updatedPosition;

            return true;
        }

        return false;
    }

    public override void Reset()
    {
        position = reverse
            ? collection.GetItems().Count - 1
            : 0;
    }
}