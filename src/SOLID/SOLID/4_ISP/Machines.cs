//Interface Segregation Principle
//Clients should not be forced to depend on methods they do not use.

namespace SOLID._4_ISP;

#region e1 v1
public class Document { }

public interface IMachine
{
    void Print(Document d);
    void Fax(Document d);
    void Scan(Document d);
}

public class MyFavoritePrinter : IMachine
{
    public void Fax(Document d)
    {
    }

    public void Print(Document d)
    {
    }

    public void Scan(Document d)
    {
    }
}

// wrong
// implements interface with methods that are not needed for this class
public class OldFashionedPrinter : IMachine
{
    public void Print(Document d)
    {
        //doing stuff
    }

    // Breaks POLA (Principle of least astonishment)
    [Obsolete("Not supported", true)]
    public void Fax(Document d)
    {
        throw new System.NotImplementedException();
    }
    // Breaks POLA (Principle of least astonishment)
    [Obsolete("Not supported", true)]
    public void Scan(Document d)
    {
        //left not implemented on purpose
    }
}
#endregion

#region e1 v2
public interface IPrinter
{
    void Print(Document d);
}
public interface IScanner
{
    void Scan(Document d);
}

public class Printer : IPrinter
{
    public void Print(Document d)
    {
        //implementation here
    }
}

//Good
public class Photocopier : IPrinter, IScanner
{
    public void Print(Document d) { }
    public void Scan(Document d) { }
}
#endregion