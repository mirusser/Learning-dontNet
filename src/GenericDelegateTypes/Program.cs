
using System.Linq.Expressions;

//delegate = More or less a variable,
//but rather than storing data,
//it stores one or more methods,
//provided they adhere to the exact signature.

//More formal (more correct and longer version):
//https://learn.microsoft.com/en-US/dotnet/csharp/programming-guide/delegates/

//A delegate is a type that represents references to methods
//with a particular parameter list and return type.
//When you instantiate a delegate, you can associate its instance with
//any method with a compatible signature and return type.
//You can invoke (or call) the method through the delegate instance.

//Delegates are used to pass methods as arguments to other methods.
//Event handlers are nothing more than methods that are invoked through delegates.
//You create a custom method, and a class such as a windows control
//can call your method when a certain event occurs. 

#region Action
#region No parameters
// represents a function
// that takes no parameters and returns nothing (void)

Action action1 = delegate { Console.WriteLine("Action 1"); };
action1();
FunctionWithOnComplete(action1);

void FunctionWithOnComplete(Action onComplete)
{
    //Do actual work:
    Console.WriteLine("FunctionWithOnComplete: actual work");

    onComplete?.Invoke();
}
#endregion No parameters

#region One parameter
// represents a function
// that takes int as a parameter
Action<int> action2 = delegate (int param1)
{
    Console.WriteLine($"Action 2, parameter: {param1}");
};
action2(10);

//same as above using lambda expression
Action<int> action2A = (int param1)
    => Console.WriteLine($"Action 2, parameter: {param1}");
action2A(10);

// for one parameter you can get rid of parentheses
Action<int> action2AB = param1
    => Console.WriteLine($"Action 2, parameter: {param1}");
action2AB(10);
#endregion One parameter

#region Two and more parameters
// represents a function
// that takes int as float as a parameters
Action<int, float> action3 = delegate (int param1, float param2)
{
    Console.WriteLine($"Action 3, parameter1: {param1}, parameter2:{param2}");
};
action3(12, 2.1f);

//same as above using lambda
Action<int, float> action3A = (int param1, float param2)
    => Console.WriteLine($"Action 3, parameter1: {param1}, parameter2:{param2}");
action3A(12, 2.1f);

//same as above but lambda can figure out the types of the parameters on its own
//there are not needed cause you specify parameters types in Action
Action<int, float> action3AB = (param1, param2)
    => Console.WriteLine($"Action 3, parameter1: {param1}, parameter2:{param2}");
action3AB(12, 2.1f);

Action<int, float> action3B = ActionExampleFunction;
action3B(14, 5.4f);

void ActionExampleFunction(int param1, float param2)
{
    Console.WriteLine($"ActionExampleFunction, parameter1: {param1}, parameter2: {param2}");
}
#endregion Three and more parameters
#endregion Action

#region Func

//Func is basically an Action that returns something
//last value in <..> is a return type, and the values before last value are parameters

//no parameter, return int
Func<int> func1 = delegate { return 1; };
int a = func1();

//int parameter, returns int
Func<int, int> func2 = delegate (int param1) { return param1 + 1; };
int b = func2(1);

// same as above using lambda expression
Func<int, int> func2A = (int param1) => param1 + 1;
int bA = func2A(1);

//int and float parameters, returns int
Func<int, float, int> func3 = delegate (int param1, float param2)
{
    return param1 + ((int)param2) + 1;
};
int c = func3(1, 1.1f);

// same as above using lambda expression
Func<int, float, int> func3A = (int param1, float param2)
    => param1 + ((int)param2) + 1;
int cA = func3(1, 1.1f);

// same as above using lambda expression but we can get rid of parameters type (explicilty)
Func<int, float, int> func3AB = (param1, param2)
    => param1 + ((int)param2) + 1;
int cAB = func3(1, 1.1f);

Func<int, float, int> func3B = FuncExampleFunction;
int d = func3B(1, 1.1f);

int FuncExampleFunction(int param1, float param2)
{
    return param1 + ((int)param2) + 1;
}

#endregion Func

#region Predicate
//Predicate is a specific 'version' of Func, takes given (only one) parameter type
//and always returns a bool
//special version of: Func<int, bool>

Predicate<int> pred1 = delegate (int param1) { return true; };
//same as above using lambda expression
Predicate<int> pred1A = (int param1) => true;
bool b1 = pred1(1);
#endregion Predicate

#region Expression trees
// (10 +20) - (5 + 3):

BinaryExpression be1 = Expression.MakeBinary(
    ExpressionType.Add,
    Expression.Constant(10),
    Expression.Constant(20));

BinaryExpression be2 = Expression.MakeBinary(
    ExpressionType.Add,
    Expression.Constant(5),
    Expression.Constant(3));

BinaryExpression be3 = Expression.MakeBinary(
    ExpressionType.Subtract,
    be1,
    be2);

int result = Expression.Lambda<Func<int>>(be3).Compile()();
#endregion Expression trees

//you can create your own (non generic) delegates
//but this is rarely used nowadays
public delegate int PerformCalculation(int x, int y);