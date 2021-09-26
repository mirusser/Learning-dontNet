using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Publisher.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TicketController : ControllerBase
    {
        private readonly IBus _bus;

        public TicketController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            if (ticket is null)
                return BadRequest();

            ticket.BookedOn = DateTime.Now;

            //naming a queue
            var uri = new Uri("rabbitmq://localhost/ticketQueue");

            //creates a queus (if not exists) with given name
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(ticket);

            return Ok();
        }
    }
}