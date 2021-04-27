using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.WebSockets;
using System.Threading;

namespace WebSocketServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                WriteRequestParam(context);

                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    Console.WriteLine("WebSocket connected");

                    await ReceiveMessage(webSocket, async (result, buffer) =>
                    {
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            Console.WriteLine("Messagge received");
                            return;
                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            Console.WriteLine("Received close message");
                            return;
                        }
                    });
                }
                else
                {
                    Console.WriteLine("Hello from the the 2nd request delegate.");
                    await next();
                }
            });

            app.Run(async (context) =>
            {
                Console.WriteLine("Hello from the the 3rd request delegate.");
                await context.Response.WriteAsync("Hello from the the 3rd request delegate.");
            });
        }

        public void WriteRequestParam(HttpContext context)
        {
            Console.WriteLine($"Request method: {context.Request.Method}");
            Console.WriteLine($"Request method: {context.Request.Protocol}");

            if (context.Request.Headers != null)
            {
                foreach (var header in context.Request.Headers)
                {
                    Console.WriteLine($" --> {header.Key} : {header.Value}");
                }
            }
        }

        private async Task ReceiveMessage(
            WebSocket socket,
            Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result =
                    await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}
