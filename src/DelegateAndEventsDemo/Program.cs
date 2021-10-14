using DelegateAndEventsDemo.Examples;
using DelegateAndEventsDemo.RealisticExample;

namespace DelegateAndEventsDemo
{
    //Delegates:
    //It's a type in C#. It represents a reference to a method (like a function pointer)
    //It's declared with a list of parameters (zero or many) and return a type (can be void)
    //When a delegate is instantiated, we can map it with any method with compatible signature
    //To call the unerlying method, we can use the Invoke function on the delegate or just call it wihe delegateinstance() signature

    //Events:
    //The event keyword is used to declare event in C#
    //An event is always associated with a delegate
    //When an event is raised, the delgate is called back
    //The delegate associated with an event ususally have to parameters as a standard practice (its no mandatory, it can have more parameters)
    //- the first one is an object representing the instance that raised the event
    //- the second one is a type representing event arguments

    //Event essentialy is a mechanism for communication between objects
    //is used in building loosely coupled applications and helps extending applications
    internal static class Program
    {
        private static void Main(string[] args)
        {
            DelegateExample.Run();
            EventExample.Run();
            MoreRealisticExample.Run();
        }
    }
}