namespace EventsAreObsolete.ConsoleApp;

public class ButtonMaster
{
    public event EventHandler<ButtonPressedEventArga>? ButtonPressed;

    public void OnButtonPressed(char keyCode)
    {
        ButtonPressed?.Invoke(this, new(keyCode));
    }
}

public class ButtonPressedEventArga
{
    public ButtonPressedEventArga(char keyCode)
    {
        KeyCode = keyCode;
    }

    public char KeyCode { get; }
}