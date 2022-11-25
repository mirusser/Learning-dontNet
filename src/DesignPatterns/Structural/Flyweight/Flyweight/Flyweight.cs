using Newtonsoft.Json;

namespace Flyweight;

// The Flyweight stores a common portion of the state (also called intrinsic
// state) that belongs to multiple real business entities. The Flyweight
// accepts the rest of the state (extrinsic state, unique for each entity)
// via its method parameters.
public class Flyweight
{
    private readonly Car sharedState;

    public Flyweight(Car car)
    {
        this.sharedState = car;
    }

    public void Operation(Car uniqueState)
    {
        string s = JsonConvert.SerializeObject(this.sharedState);
        string u = JsonConvert.SerializeObject(uniqueState);
        Console.WriteLine($"Flyweight: Displaying shared {s} and unique {u} state.");
    }
}