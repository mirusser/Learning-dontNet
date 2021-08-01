using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDemo.Data;
using GraphQLDemo.GprahQL.Platforms;
using GraphQLDemo.Models;
using HotChocolate;
using HotChocolate.Data;

namespace GraphQLDemo.GprahQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddPlatformPayload> AddPlatformAsync(AddPlatformInput input, [ScopedService] AppDbContext context)
        {
            var platform = new Platform
            {
                Name = input.Name
            };

            await context.Platforms.AddAsync(platform);
            await context.SaveChangesAsync();

            return new AddPlatformPayload(platform);
        }
    }
}
