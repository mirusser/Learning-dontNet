using System.Threading.Tasks;
using EcommService.Providers;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.EventModels;

namespace EcommService.Listeners
{
    public class OrderCreatedListener : IConsumer<OrderCreated>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        private readonly ILogger<OrderCreatedListener> _logger;
        private readonly IInventoryUpdatorProvider _inventoryUpdatorProvider;

        public OrderCreatedListener(
            IPublishEndpoint publishEndpoint,
            ILogger<OrderCreatedListener> logger,
            IInventoryUpdatorProvider inventoryUpdatorProvider)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _inventoryUpdatorProvider = inventoryUpdatorProvider;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            _logger.LogInformation("Got order created");

            var numberOfRowsAffected =
                await _inventoryUpdatorProvider.UpdateAsync(context.Message.ProductId, context.Message.Quantity);

            var inventoryResponse = new InventoryResponse
            {
                OrderId = context.Message.OrderId,
                IsSuccess = numberOfRowsAffected > 0
            };

            _logger.LogInformation("Sending/publishing a message: {InventoryResponse}", inventoryResponse);

            await _publishEndpoint.Publish<InventoryResponse>(inventoryResponse);
        }
    }
}