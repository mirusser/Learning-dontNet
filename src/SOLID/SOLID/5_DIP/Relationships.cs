//High level classes (modules)
//shouldn't depend on low-level classes (modules)
// both groups should depend on abstraction.

//Abstractions should not depend on details
//and details should depend on abstractions

namespace SOLID._5_DIP;

#region e1 v1

public enum Relationship
{
    Parent,
    Child,
    Sibling
}

public class Person
{
    public string? Name { get; set; }
    // Other properties here...
}

public class Relationships //low-level
{
    public List<(Person, Relationship, Person)> Relations { get; } = new();

    public void AddParentAndChild(Person parent, Person child)
    {
        Relations.Add((parent, Relationship.Parent, child));
        Relations.Add((child, Relationship.Child, parent));
    }
}

public class Research
{
    public Research(Relationships relationships)
    {
        //high-level: find all Jan's children

        var relations = relationships.Relations;
        var janAsAParentRelations = relations
            .Where(x =>
                x.Item1.Name == "Jan"
                && x.Item2 == Relationship.Parent);

        foreach (var relation in janAsAParentRelations)
        {
            Console.WriteLine($"Jan's child: {relation.Item3.Name}");
        }
    }
}

//what if we wanted to change the mechanism
//of storing data of Relationships class
//from Tuple to proper DataBase class model?

#endregion

#region e1 v2

public interface IRelationshipBrowser
{
    IEnumerable<Person> FindAllChildrenOf(string name);
}
public class RelationshipsV2 : IRelationshipBrowser //low-level
{
    //this method is no longer public
    //keeping secrets hidden helps to extract proper interface
    private readonly List<(Person, Relationship, Person)> relations = new();

    public IEnumerable<Person> FindAllChildrenOf(string name)
    {
        return relations
            .Where(x =>
                x.Item1.Name == name
                && x.Item2 == Relationship.Parent)
            .Select(r => r.Item3);
    }
}

public class ResearchV2
{
    public ResearchV2(IRelationshipBrowser browser)
    {
        foreach (var p in browser.FindAllChildrenOf("Jan"))
        {
            Console.WriteLine($"Jan has a child: {p.Name}");
        }
    }
}

#endregion