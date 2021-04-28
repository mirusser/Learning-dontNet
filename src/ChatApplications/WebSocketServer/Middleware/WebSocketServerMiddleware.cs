using System;
using System.Net.WebSockets;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebSocketServer.Managers;
using System.Text.Json;
using Newtonsoft.Json;

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
                await SendConnIdAsync(webSocket, connId);

                await ReceiveMessageAsync(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Message received: {message}");
                        await RouteJSONMessageAsync(message);
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        Console.WriteLine("Received close message");

                        string id = _manager.GetAllSockets().FirstOrDefault(s => s.Value == webSocket).Key;

                        _manager.GetAllSockets().TryRemove(id, out WebSocket socket);

                        await socket.CloseAsync(
                            result.CloseStatus.Value, 
                            result.CloseStatusDescription, 
                            CancellationToken.None);

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

        private async Task SendConnIdAsync(WebSocket socket, string connId)
        {
            var buffer = Encoding.UTF8.GetBytes($"ConnId: {connId}");

            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private void WriteRequestParam(HttpContext context)
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

        private async Task ReceiveMessageAsync(
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

        public async Task RouteJSONMessageAsync(string message)
        {
            //var routeOb = System.Text.Json.JsonSerializer.Deserialize<dynamic>(message);
            var routeOb = JsonConvert.DeserializeObject<dynamic>(message);

            if (routeOb.To != null && Guid.TryParse(routeOb.To.ToString(), out Guid guidOutput))
            {
                Console.WriteLine("Targeted message");
                var socket = _manager.GetAllSockets().FirstOrDefault(s => s.Key == routeOb.To.ToString());

                if (socket.Value != null)
                {
                    if (socket.Value.State == WebSocketState.Open)
                    {
                        await socket.Value.SendAsync(
                            Encoding.UTF8.GetBytes(routeOb.Message.ToString()),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid recipent");
                }
            }
            else
            {
                Console.WriteLine("Broadcast");
                foreach (var socket in _manager.GetAllSockets())
                {
                    if (socket.Value.State == WebSocketState.Open)
                    {
                        await socket.Value.SendAsync(
                            Encoding.UTF8.GetBytes(routeOb.Message.ToString()),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    }
                }
            }
        }
    }
}
