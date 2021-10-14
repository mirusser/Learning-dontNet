using System;

namespace DelegateAndEventsDemo
{
    public class CalculatorDelegatesExample
    {
        //delegate kinda behave like as if you declared an inner class (it doesn't become a member of the class)
        //It's more like an inner type than instance member
        public delegate int Calculate(int input);

        public int Execute(Calculate calculate, int input)
        {
            //return calculate.Invoke(input); // other way of using delegate
            return calculate(input);
        }

        //Same as 'Execute' but you don't have to create your own delegate
        //Func returns something
        public int ExecuteFunc(Func<int, int> calculate, int input)
        {
            return calculate(input);
        }

        //Func doesn't return anything
        public void ExecuteAction(Action<int> print, int input)
        {
            print(input);
        }

        //with Func and Action there is no real reason to create your own delegates, cause this to delegates cover most cases
    }

    public class CalculatorEventsExample
    {
        public event Action<object, CalculatorEventArgs> Calculate;

        public void RaiseEvent(string name)
        {
            Calculate?.Invoke(this, new CalculatorEventArgs { Name = name });
        }
    }
}