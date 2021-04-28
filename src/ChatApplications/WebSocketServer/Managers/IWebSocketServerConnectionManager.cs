using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketServer.Managers
{
    public interface IWebSocketServerConnectionManager
    {
        string AddSocket(WebSocket socket);
        ConcurrentDictionary<string, WebSocket> GetAllSockets();
    }
}