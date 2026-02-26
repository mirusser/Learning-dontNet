using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Threading;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloWorld
{
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

        public OutputEvent FunctionHandler(InputEvent input)
        {
            var message = $"{input.Message} {input.FirstName} {input.LastName}";
            Console.WriteLine(message);
            
            return new OutputEvent
            {
                Message = message 
            };
        }
    }

    public class InputEvent
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = "";
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = "";
        [JsonPropertyName("message")]
        public string Message { get; set; } = "";
    }

    public class OutputEvent
    {
        public string Message { get; set; } = "";
    }
}