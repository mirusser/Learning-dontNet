using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderService.Models;
using OrderService.Providers;
using Shared.Models;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderDetailsProvider _orderDetailsProvider;
        private readonly IOrderCreatorProvider _orderCreatorProvider;

        private readonly ILogger<OrderController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(
            IOrderDetailsProvider orderDetailsProvider,
            IOrderCreatorProvider orderCreatorProvider,
            ILogger<OrderController> logger,
            IPublishEndpoint publishEndpoint)
        {
            _orderDetailsProvider = orderDetailsProvider;
            _orderCreatorProvider = orderCreatorProvider;

            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderDetail>> Get()
        {
            return await _orderDetailsProvider.GetAll();
        }

        [HttpGet("{id}")]
        public string Get(int id) //TODO
        {
            return "value";
        }

        [HttpPost]
        public async Task Post([FromBody] OrderDetail orderDetail)
        {
            var id = await _orderCreatorProvider.Create(orderDetail);

            var orderCreated = new OrderCreated
            {
                OrderId = id,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity
            };

            _logger.LogInformation("Started sending/publishing a message...");

            await _publishEndpoint.Publish<OrderCreated>(orderCreated);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}