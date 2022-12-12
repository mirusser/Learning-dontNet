using Helpers;

namespace SimpleMath;

public static class IsPrime
{
    public static void ConsoleRun()
    {
        int n = UserInputHelper.GetInteger("Enter number to test:");

        if (Run(n))
        {
            Console.WriteLine("Prime");
        }
        else
        {
            Console.WriteLine("Not prime");
        }
    }

    /// <summary>
    /// 1. First, check for some special cases.
    /// If the number is less than or equal to 1, it is not prime.
    /// If the number is 2 or 3, it is prime.
    /// 2. If the number is not one of the special cases, check if it is divisible by 2 or 3.
    /// If it is, the number is not prime.
    /// 3. If the number is not divisible by 2 or 3, use a loop to
    /// check if it is divisible by any number between 5 and the square root of the number.
    /// If the number is divisible by any of these numbers, it is not prime.
    /// 4. If the number is not divisible by any of these numbers, it is prime.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static bool Run(int n)
    {
        if (n <= 1)
        {
            return false;
        }
        if (n <= 3)
        {
            return true;
        }
        if (n % 2 == 0 || n % 3 == 0)
        {
            return false;
        }
        else
        {
            for (int i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}