using Helpers;

namespace SimpleMath;

public static class IsDividable
{
    public static void Run()
    {
        int numberToTest = UserInputHelper.GetInteger("Enter number to test:");
        int divider = UserInputHelper.GetInteger("Enter divider:");

        if (numberToTest % divider == 0)
        {
            Console.WriteLine($"{numberToTest} is dividable by {divider}");
        }
        else
        {
            Console.WriteLine($"{numberToTest} is not dividable by {divider}");
        }
    }
}