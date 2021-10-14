using System;

namespace DelegateAndEventsDemo.Examples
{
    public static class DelegateExample
    {
        private static int Square(int input) => input * input;

        public static void Run()
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
    }
}