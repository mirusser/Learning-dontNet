using EasyConsole;
using SimpleMath;

static void DisplayOption(Action callback)
{
    Console.Clear();

    callback.Invoke();

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

var isExit = false;
do
{
    new Menu()
          .Add("Is dividable?", () => DisplayOption(() => IsDividable.Run()))
          .Add("Is prime?", () => DisplayOption(() => IsPrime.Run()))
          .Add("Exit", () => isExit = true)
          .Display();

    Console.Clear();
} while (!isExit);