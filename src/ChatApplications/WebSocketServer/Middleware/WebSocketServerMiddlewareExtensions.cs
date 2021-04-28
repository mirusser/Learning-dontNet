using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebSocketServer.Managers;

namespace WebSocketServer.Middleware
{
    public static class WebSocketServerMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketServerMiddleware>();
        }

        public static IServiceCollection AddWebSocketManger(this IServiceCollection services)
        {
            services.AddSingleton<IWebSocketServerConnectionManager, WebSocketServerConnectionManager>();

            return services;
        }
    }
}
