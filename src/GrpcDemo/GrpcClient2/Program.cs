using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var request = new HelloRequest()
            {
                Name = "Jon"
            };

            var response = await client.SayHelloAsync(request);

            Console.WriteLine($"Response: {response.Message}");

            using (var call = client.SayHelloStream(request))
            {
                await foreach (var helloReply in call.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine($"{helloReply.Message}");
                }
            }

            Console.ReadLine();
        }
    }
}
