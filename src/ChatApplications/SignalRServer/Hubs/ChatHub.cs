using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SignalRServer.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Connection Established, connection Id: {Context.ConnectionId}");

            Clients.Client(Context.ConnectionId).SendAsync($"Received connection Id: {Context.ConnectionId}");

            return base.OnConnectedAsync();
        }

        public async Task SendMessageAsync(string message)
        {
            Console.WriteLine($"Message Recieved on: {Context.ConnectionId}");

            var routeOb = JsonConvert.DeserializeObject<dynamic>(message);
            string toClient = routeOb.To;

            if (!string.IsNullOrEmpty(toClient))
            {
                await Clients.Client(toClient).SendAsync("ReceiveMessage", message);
            }
            else
            {
                await Clients.All.SendAsync("ReceiveMessage", message);
            }
        }
    }
}
