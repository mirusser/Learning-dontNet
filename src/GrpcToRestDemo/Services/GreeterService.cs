using Grpc.Core;
using GrpcToRestDemo;

namespace GrpcToRestDemo.Services;

public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(
            HelloRequest request,
            ServerCallContext context)
    {
        logger.LogInformation("Saying hello");

        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}