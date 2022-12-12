using Helpers;

namespace SimpleMath;

public static class IsPrime
{
    public static void Run()
    {
        int n = UserInputHelper.GetInteger("Enter number to test:");

        if (n < 2)
        {
            Console.WriteLine("Not prime");
        }
        else if (n == 2)
        {
            Console.WriteLine("Prime");
        }
        else if (n % 2 == 0)
        {
            Console.WriteLine("Not prime");
        }
        else
        {
            for (int i = 3; i < Math.Sqrt(n) + 1; i += 2)
            {
                if (n % i == 0)
                {
                    Console.WriteLine("Not prime");
                    return;
                }
            }

            Console.WriteLine("Prime");
        }
    }
}