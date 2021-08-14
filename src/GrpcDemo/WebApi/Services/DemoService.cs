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

namespace WebApi.Services
{
    public class DemoService : IHostedService, IDemoService
    {
        private readonly Greeter.GreeterClient _client;

        //public DemoService(Greeter.GreeterClient client)
        //{
        //    //_client = client;
        //}

        public DemoService(GrpcClientFactory grpcClientFactory)
        {
            _client = grpcClientFactory.CreateClient<Greeter.GreeterClient>("Greeter");
        }

        public async Task SayHello()
        {
            var request = new HelloRequest()
            {
                Name = "Jon"
            };

            try
            {
                var response = await _client.SayHelloAsync(request, deadline: DateTime.UtcNow.AddSeconds(5));

                Console.WriteLine($"Response: {response.Message}");
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                Console.WriteLine("Say hello timeout");
            }
            catch (Exception ex)
            {
                var foo = ex;
            }
        }

        public async IAsyncEnumerable<HelloReply> SayHellos()
        {
            var request = new HelloRequest()
            {
                Name = "Jon"
            };

            using var call = _client.SayHelloStream(request, deadline: DateTime.UtcNow.AddSeconds(15));

            await foreach (var helloReply in call.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"{helloReply.Message}");
                yield return helloReply;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SayHello();
            await foreach (var helloReply in SayHellos()) ;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
