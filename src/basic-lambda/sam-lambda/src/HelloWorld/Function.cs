using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloWorld;

public class Function
{
    public Function()
    {
        // Kick off the debug attach
        // Launch will prompt the OS to open a debugger; if you have VS Code or Visual Studio listening on a remote port,
        // this will cause it to offer to attach.
        if (!Debugger.IsAttached)
        {
            Console.WriteLine("⏱ Launching debugger…");
            Debugger.Launch();
        }

        // Wait until the debugger is really attached
        while (!Debugger.IsAttached)
        {
            Console.WriteLine("⏱ Waiting for debugger attach…");
            Thread.Sleep(500);
        }

        Console.WriteLine("✅ Debugger attached.");
    }

    public OutputEvent Handle(InputEvent input)
    {
        return new OutputEvent {
            Message = $"{input.Message} {input.FirstName} {input.LastName}"
        };
    }
}

public class InputEvent
{
    public string FirstName { get; set; } = "";
    public string LastName  { get; set; } = "";
    public string Message   { get; set; } = "";
}

public class OutputEvent
{
    public string Message { get; set; } = "";
}