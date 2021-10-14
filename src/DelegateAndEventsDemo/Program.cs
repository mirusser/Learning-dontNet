using System;

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
    internal static class Program
    {
        private static void Main(string[] args)
        {
            RunDelegateExamples();
            RunEventExamples();
        }

        private static int Square(int input) => input * input;

        private static void RunDelegateExamples()
        {
            var calculator = new CalculatorDelegatesExample();

            CalculatorDelegatesExample.Calculate calc = Square;
            var result = calculator.Execute(calc, 5);

            Console.WriteLine($"Result: {result}");

            var secondResult = calculator.Execute(Square, 5);

            Console.WriteLine($"SecondResult: {secondResult}");

            calculator.ExecuteAction(Console.WriteLine, 25);

            //example of using Func with lambda
            //Lamba expression is used to create anonymous function
            //Lambda can represent any delegate type (the parameters and return type has to match)
            //in this example is the exact same as using 'Square' function
            //with lambdas there is no much reason to create, small non reusable functions
            //delegate representation in most cases ended up being a lambda
            var thirdResult = calculator.ExecuteFunc((x) => x * x, 5);

            Console.WriteLine($"ThirdResult: {thirdResult}");
        }

        private static void RunEventExamples()
        {
            var calculator = new CalculatorEventsExample();

            calculator.Calculate += Calculator_Calculate;

            calculator.RaiseEvent("Test name"); //first event is raised here

            calculator.Calculate += (obj, args) => Console.WriteLine($"Second event: {args.Name}");

            calculator.RaiseEvent("Second Test name"); //first and second event are raised here
        }

        private static void Calculator_Calculate(object arg1, CalculatorEventArgs arg2)
        {
            Console.WriteLine($"First event: {arg2.Name}");
        }
    }
}