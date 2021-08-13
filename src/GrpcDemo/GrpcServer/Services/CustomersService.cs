using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services
{
    public class CustomersService : Customers.CustomersBase
    {
        private readonly ILogger<CustomersService> logger;

        public CustomersService(ILogger<CustomersService> logger)
        {
            this.logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new();

            //some mock data (simulation of getting data)
            if (request.UserId == 1)
            {
                output.FirstName = "Jamie";
                output.LastName = "Smith";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Jane";
                output.LastName = "Doe";
            }
            else
            {
                output.FirstName = "Greg";
                output.LastName = "Thomas";
            }

            return Task.FromResult(output);
        }

        public override async Task GetNewCustomers(
            NewCustomeRequest request,
            IServerStreamWriter<CustomerModel> responseStream,
            ServerCallContext context)
        {
            List<CustomerModel> customers = new()
            {
                new CustomerModel
                {
                    FirstName = "Jon",
                    LastName = "Doe",
                    EmailAddress = "mail@mail.com",
                    Age = 42,
                    IsAlive = true
                },
                new CustomerModel
                {
                    FirstName = "Jon2",
                    LastName = "Doe2",
                    EmailAddress = "mail2@mail.com",
                    Age = 23,
                    IsAlive = false
                },
                new CustomerModel
                {
                    FirstName = "Jon3",
                    LastName = "Doe3",
                    EmailAddress = "mail3@mail.com",
                    Age = 45,
                    IsAlive = false
                }
            };

            foreach (var customer in customers)
            {
                await responseStream.WriteAsync(customer);
            }
        }
    }
}
