using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQLDemo.Data;
using GraphQLDemo.GprahQL.Commands;
using GraphQLDemo.GprahQL.Platforms;
using GraphQLDemo.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Subscriptions;

namespace GraphQLDemo.GprahQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddPlatformPayload> AddPlatformAsync(
            AddPlatformInput input, 
            [ScopedService] AppDbContext context,
            [Service] ITopicEventSender eventSender, // used to subscribe to 'OnPlatformAdded'
            CancellationToken cancelletionToken)
        {
            Platform platform = new()
            {
                Name = input.Name
            };

            await context.Platforms.AddAsync(platform, cancelletionToken);
            await context.SaveChangesAsync(cancelletionToken);

            await eventSender.SendAsync(nameof(Subscription.OnPlatformAdded), platform, cancelletionToken);

            return new AddPlatformPayload(platform);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddCommandPayload> AddCommandAsync(AddCommandInput input, [ScopedService] AppDbContext context)
        {
            Command command = new()
            {
                HowTo = input.HowTo,
                CommandLine= input.CommandLine,
                PlatformId = input.PlatformId
            };

            await context.Commands.AddAsync(command);
            await context.SaveChangesAsync();

            return new AddCommandPayload(command);
        }
    }
}
