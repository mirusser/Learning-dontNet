using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Providers;
using Shared.EventModels;

namespace OrderService.Listeners
{
    public class InventoryResponseListener : IConsumer<InventoryResponse>
    {
        private readonly ILogger<InventoryResponseListener> _logger;
        private readonly IOrderDeletorProvider _orderDeletorProvider;

        public InventoryResponseListener(
            ILogger<InventoryResponseListener> logger,
            IOrderDeletorProvider orderDeletorProvider)
        {
            _logger = logger;
            _orderDeletorProvider = orderDeletorProvider;
        }

        public async Task Consume(ConsumeContext<InventoryResponse> context)
        {
            _logger.LogInformation("Got inventory response");

            if (context.Message.IsSuccess)
            {
                await _orderDeletorProvider.DeleteAsync(context.Message.OrderId);
            }
        }
    }
}