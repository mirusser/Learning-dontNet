using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var helloRequest = new HelloRequest { Name = "Jon" };
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);
            //var reply = await client.SayHelloAsync(helloRequest);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var customerClient = new Customers.CustomersClient(channel);

            var clientRequested = new CustomerLookupModel() { UserId = 2 };
            var customer = await customerClient.GetCustomerInfoAsync(clientRequested);

            Console.WriteLine($"{customer.FirstName} {customer.LastName}");

            Console.WriteLine("New customer list:");
            using (var call = customerClient.GetNewCustomers(new NewCustomeRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;

                    Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName} {currentCustomer.EmailAddress}");
                }
            }

            Console.WriteLine("Second new customer list:");
            using (var call = customerClient.GetNewCustomers(new NewCustomeRequest()))
            {
                await foreach (var currentCustomer in call.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine($"{currentCustomer.FirstName} {currentCustomer.LastName}: {currentCustomer.EmailAddress}");
                }
            }

            Console.ReadLine();
        }
    }
}
