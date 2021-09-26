using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Shared.Models;

namespace Consumer.Consumers
{
    public class TicketConsumer : IConsumer<Ticket>
    {
        public async Task Consume(ConsumeContext<Ticket> context)
        {
            var data = context.Message;

            //TODO: after getting/consuming the message (that ticket was created)
            //validate the ticket data
            //store to database
            //notify the user via email/sms
        }
    }
}