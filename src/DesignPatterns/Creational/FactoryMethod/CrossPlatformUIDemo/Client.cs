using System;
using CrossPlatformUIDemo.Creators;

namespace CrossPlatformUIDemo;

public static class Client
{
    public static void Do()
    {
        Console.WriteLine("Pass button type (w or h)");
        var key = Console.ReadKey();
        Console.WriteLine();

        Dialog? dialog = null;

        switch (key.KeyChar)
        {
            case 'w':
                dialog = new WindowsDialog();
                break;
            case 'h':
                dialog = new WebDialog();
                break;
            default:
                Console.WriteLine("No button for this key");
                break;
        }

        dialog?.Render();
    }
}