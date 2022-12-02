using Mediator.Components;

namespace Mediator;

// Concrete Mediators implement cooperative behavior by coordinating several
// components.
internal class ConcreteMediator : IMediator
{
    private Component1 component1;

    private Component2 component2;

    public ConcreteMediator(
        Component1 component1,
        Component2 component2)
    {
        this.component1 = component1;
        this.component1.SetMediator(this);
        this.component2 = component2;
        this.component2.SetMediator(this);
    }

    public void Notify(object sender, string ev)
    {
        if (ev == "A")
        {
            Console.WriteLine("Mediator reacts on A and triggers folowing operations:");
            component2.DoC();
        }
        if (ev == "D")
        {
            Console.WriteLine("Mediator reacts on D and triggers following operations:");
            component1.DoB();
            component2.DoC();
        }
    }
}