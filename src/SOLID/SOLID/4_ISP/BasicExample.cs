//Interface Segregation Principle
//Clients should not be forced to depend on methods they do not use.

namespace SOLID._4_ISP;

public interface IReadable
{
    // This interface defines methods for reading data.

    string Read();

    string ReadLine();
}

public interface IWriteable
{
    // This interface defines methods for writing data.

    void Write(string data);

    void WriteLine(string data);
}

public class File : IReadable, IWriteable
{
    // This class implements both interfaces, but it only
    // needs to implement the methods it uses.

    public string Read()
    {
        // Code to read data from a file.
        return string.Empty;
    }

    public string ReadLine()
    {
        // Code to read a line from a file.
        return string.Empty;
    }

    public void Write(string data)
    {
        // Code to write data to a file.
    }

    public void WriteLine(string data)
    {
        // Code to write a line to a file.
    }
}