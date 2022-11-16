namespace Builder;

// It makes sense to use the Builder pattern only when your products are
// quite complex and require extensive configuration.
//
// Unlike in other creational patterns, different concrete builders can
// produce unrelated products. In other words, results of various builders
// may not always follow the same interface.
public class Product
{
    private readonly List<object> parts = new();

    public void Add(string part)
    {
        parts.Add(part);
    }

    public string ListParts()
    {
        string str = string.Empty;

        for (int i = 0; i < parts.Count; i++)
        {
            str += parts[i] + ", ";
        }

        str = str.Remove(str.Length - 2); // removing last ",c"

        return "Product parts: " + str + "\n";
    }
}