//Single Responsibility Principle
//A class should have only one reason to change/only one responsibility.

using System.Reflection;

namespace SOLID._1_SRP;

#region e1 v1

public class Journal_V1
{
    private readonly List<string> entries = new();

    private static int count = 0;

    public void AddEntry(string text)
    {
        entries.Add($"{++count}: {text}");
    }
    public void RemoveEntry(int index)
    {
        entries.RemoveAt(index);
    }
}

public class JurnalClient_V1 : IClient
{
    public void DoSomething()
    {
        Journal_V1 journal = new();
        journal.AddEntry("I cried a bit.");
        journal.AddEntry("I ate a wobbly worm.");
    }
}

#endregion

#region e1 v2

public class Journal_V2
{
    private readonly List<string> entries = new();

    private static int count = 0;

    public void AddEntry(string text)
    {
        entries.Add($"{++count}: {text}");
    }

    public void RemoveEntry(int index)
    {
        entries.RemoveAt(index);
    }

    // new feature
    // (in other words new functionality that extends the original scope of the class).
    // Journal should keep entries, not save them on disk
    public void Save(string filename, bool overwrite = false)
    {
        File.WriteAllText(filename, ToString());
    }
}

public class JurnalClient_V2 : IClient
{
    public void DoSomething()
    {
        Journal_V2 journal = new();
        journal.AddEntry("I cried a bit.");
        journal.AddEntry("I ate a wobbly worm.");

        var executionPath = Assembly.GetExecutingAssembly().Location;
        const string filename = "journal";
        var fullPath = Path.Combine(executionPath, filename);
        journal.Save(fullPath);
    }
}

#endregion

#region e1 v3

public class Journal_V3
{
    private readonly List<string> entries = new();

    private static int count = 0;

    public void AddEntry(string text)
    {
        entries.Add($"{++count}: {text}");
    }

    public void RemoveEntry(int index)
    {
        entries.RemoveAt(index);
    }
}

// Moved saving on disk to new class
public class PersistenceManager
{
    public void SaveToFile(
        Journal_V3 journal,
        string filename,
        bool overwrite = false)
    {
        if (overwrite || !File.Exists(filename))
        {
            File.WriteAllText(filename, journal.ToString());
        }
    }
}

public class JurnalClient_V3 : IClient
{
    public void DoSomething()
    {
        Journal_V3 journal = new();
        journal.AddEntry("I cried a bit.");
        journal.AddEntry("I ate a wobbly worm.");

        var executionPath = Assembly.GetExecutingAssembly().Location;
        const string filename = "journal";
        var fullPath = Path.Combine(executionPath, filename);
        PersistenceManager persistenceManager = new();
        persistenceManager.SaveToFile(journal, fullPath);
    }
}

#endregion