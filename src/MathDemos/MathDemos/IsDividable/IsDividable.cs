using Helpers;

namespace SimpleMath;

public static class IsDividable
{
    public static void ConsoleRun()
    {
        int numberToTest = UserInputHelper.GetInteger("Enter number to test:");
        int divider = UserInputHelper.GetInteger("Enter divider:");

        //there is no point moving it to its own (even local) method
        if (Run(numberToTest, divider))
        {
            Console.WriteLine($"{numberToTest} is dividable by {divider}");
        }
        else
        {
            Console.WriteLine($"{numberToTest} is not dividable by {divider}");
        }
    }

    public static bool Run(int number, int divider)
        => number % divider == 0;
}