//Liskov Substitution Principle
//Derived classes should be substitutable for their base classes.

namespace SOLID._3_LSP;

public abstract class Vehicle
{
    // This class defines a contract that derived classes
    // must adhere to.

    public abstract void StartEngine();
    public abstract void StopEngine();
    public abstract void Drive();
}

public class Car : Vehicle
{
    public override void StartEngine()
    {
        // Code to start the engine of a car.
    }

    public override void StopEngine()
    {
        // Code to stop the engine of a car.
    }

    public override void Drive()
    {
        // Code to drive a car.
    }
}

public class Bicycle : Vehicle
{
    public override void StartEngine()
    {
        // Not applicable for a bicycle.
    }

    public override void StopEngine()
    {
        // Not applicable for a bicycle.
    }

    public override void Drive()
    {
        // Code to ride a bicycle.
    }
}