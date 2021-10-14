using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateAndEventsDemo.Examples
{
    public static class EventExample
    {
        public static void Run()
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
