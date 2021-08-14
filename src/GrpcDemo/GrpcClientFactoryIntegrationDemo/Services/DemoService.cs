using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using GrpcServer;
using Microsoft.Extensions.Hosting;

namespace GrpcClientFactoryIntegrationDemo.Services
{
    public class DemoService : IHostedService, IDemoService
    {
        private readonly Greeter.GreeterClient _client;

        public DemoService(Greeter.GreeterClient client, GrpcClientFactory grpcClientFactory)
        {
            //_client = client;
            _client = grpcClientFactory.CreateClient<Greeter.GreeterClient>("Greeter");
        }

        public async IAsyncEnumerable<HelloReply> SayHellos()
        {
            var request = new HelloRequest()
            {
                Name = "Jon"
            };

            var response = await _client.SayHelloAsync(request);

            Console.WriteLine($"Response: {response.Message}");

            using var call = _client.SayHelloStream(request);

            await foreach (var helloReply in call.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"{helloReply.Message}");
                yield return helloReply;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await foreach (var helloReply in SayHellos());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
