using System;
using System.Threading.Tasks;
using MassTransit;
using Model;

namespace InventoryService
{
    internal class OrderConsumer : IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context)
        {
            await Console.Out.WriteLineAsync(context.Message.Name);
        }
    }
}