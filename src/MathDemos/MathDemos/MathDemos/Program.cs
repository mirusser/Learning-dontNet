using EasyConsole;

static void DisplayOption(Action callback)
{
    callback.Invoke();

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

var isExit = false;
do
{
    new Menu()
          .Add("Is dividable?", () => DisplayOption(() => IsDividable.IsDividable.Run()))
          .Add("Exit", () => isExit = true)
          .Display();

    Console.Clear();
} while (!isExit);