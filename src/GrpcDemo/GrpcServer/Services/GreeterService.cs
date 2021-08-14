using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServer;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = $"Hello {request.Name}"
            });
        }

        public override async Task SayHelloStream(
            HelloRequest request,
            IServerStreamWriter<HelloReply> responseStream,
            ServerCallContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                var replay = new HelloReply()
                {
                    Message = $"Hello: {request.Name} {i}"
                };
                await responseStream.WriteAsync(replay);

                //using deadline example:
                //var user =
                //    await _databaseContext.GetUserAsync(request.Name, context.CancellationToken);

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
