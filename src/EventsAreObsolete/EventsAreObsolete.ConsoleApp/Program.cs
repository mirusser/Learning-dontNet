using EventsAreObsolete.ConsoleApp;

var buttonMaster = new ButtonMaster();

buttonMaster.ButtonPressed += (sender, eventArgs) =>
{
    Console.WriteLine($"Button: {eventArgs.KeyCode} was pressed");
};

buttonMaster.ButtonPressed += (sender, eventArgs) =>
{
    Console.WriteLine($"Button: {eventArgs.KeyCode} was pressed from different handler");
};

while (true)
{
    var keyCode = Console.ReadKey(true).KeyChar;
    buttonMaster.OnButtonPressed(keyCode);
}
