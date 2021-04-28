using System;
using System.Net.WebSockets;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebSocketServer.Managers;

namespace WebSocketServer.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketServerConnectionManager _manager;

        public WebSocketServerMiddleware(
            RequestDelegate next,
            IWebSocketServerConnectionManager manager)
        {
            _next = next;
            _manager = manager;
        }

        public async Task Invoke(HttpContext context)
        {
            WriteRequestParam(context);

            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("WebSocket connected");

                string connId = _manager.AddSocket(webSocket);

                await ReceiveMessage(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        Console.WriteLine($"Message received: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
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
                await _next(context);
            }
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
                    await socket.ReceiveAsync(
                        buffer: new ArraySegment<byte>(buffer),
                        cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}
