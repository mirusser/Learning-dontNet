using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketServer.Managers
{
    public class WebSocketServerConnectionManager : IWebSocketServerConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets;

        public WebSocketServerConnectionManager()
        {
            _sockets = new ConcurrentDictionary<string, WebSocket>();
        }

        public ConcurrentDictionary<string, WebSocket> GetAllSockets()
            => _sockets;

        public string AddSocket(WebSocket socket)
        {
            var connId = Guid.NewGuid().ToString();

            _sockets.TryAdd(connId, socket);

            Console.WriteLine($"Connection added: {connId}");

            return connId;
        }
    }
}
