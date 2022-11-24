using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Facade;

// The Facade class provides a simple interface to the complex logic of one
// or several subsystems. The Facade delegates the client requests to the
// appropriate objects within the subsystem. The Facade is also responsible
// for managing their lifecycle. All of this shields the client from the
// undesired complexity of the subsystem.
public class Facade
{
    protected Subsystem1 _subsystem1;
    protected Subsystem2 _subsystem2;

    public Facade(Subsystem1 subsystem1, Subsystem2 subsystem2)
    {
        this._subsystem1 = subsystem1;
        this._subsystem2 = subsystem2;
    }

    // The Facade's methods are convenient shortcuts to the sophisticated
    // functionality of the subsystems. However, clients get only to a
    // fraction of a subsystem's capabilities.
    public string Operation()
    {
        string result = "Facade initializes subsystems:\n";
        result += Subsystem1.Operation1();
        result += Subsystem2.Operation1();
        result += "Facade orders subsystems to perform the action:\n";
        result += Subsystem1.OperationN();
        result += Subsystem2.OperationZ();
        return result;
    }
}
