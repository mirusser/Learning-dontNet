using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facade;

// Some facades can work with multiple subsystems at the same time.
public class Subsystem2
{
    public static string Operation1()
    {
        return "Subsystem2: Get ready!\n";
    }

    public static string OperationZ()
    {
        return "Subsystem2: Fire!\n";
    }
}
