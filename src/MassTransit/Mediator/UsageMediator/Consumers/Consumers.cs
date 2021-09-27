using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace UsageMediator.Consumers;

public interface GetOrderStatus
{
    Guid OrderId { get; }
}

public interface OrderStatus
{
    Guid OrderId { get; }
    string Status { get; }
}

public interface SubmitOrder
{
    public Guid Order { get; set; }
}

public class OrderStatusConsumer : IConsumer<GetOrderStatus>
{
    public async Task Consume(ConsumeContext<GetOrderStatus> context)
    {
        await context.RespondAsync<OrderStatus>(new
        {
            context.Message.OrderId,
            Status = "Pending"
        });
    }
}

public class SubmitOrderConsumer : IConsumer<SubmitOrder>
{
    public async Task Consume(ConsumeContext<SubmitOrder> context)
    {
        Console.WriteLine($"Order was submitted, order: {context.Message.Order}");
    }
}