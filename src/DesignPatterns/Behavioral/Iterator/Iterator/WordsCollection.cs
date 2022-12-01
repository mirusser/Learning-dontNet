using System.Collections;

namespace Iterator;

// Concrete Collections provide one or several methods for retrieving fresh
// iterator instances, compatible with the collection class.
public class WordsCollection : IteratorAggregate
{
    private readonly List<string> collection = new();

    private bool direction;

    public void ReverseDirection()
    {
        direction = !direction;
    }

    public List<string> GetItems()
        => collection;

    public void AddItem(string item)
    {
        collection.Add(item);
    }

    public override IEnumerator GetEnumerator()
    {
        return new AlphabeticalOrderIterator(this, direction);
    }
}