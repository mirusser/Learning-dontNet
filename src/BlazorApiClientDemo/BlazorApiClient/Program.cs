using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazorApiClient.DataServices;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorApiClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("BlazorApiClient has started...");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["apiBaseUrl"]) });
            //builder.Services.AddHttpClient<ISpaceXDataService, RestSpaceXDataService>
            //    (
            //        spdc => spdc.BaseAddress = new Uri(builder.Configuration["apiBaseUrl"])
            //    );

            builder.Services.AddHttpClient<ISpaceXDataService, GraphQLSPaceXDataService>
                (
                    spdc => spdc.BaseAddress = new Uri(builder.Configuration["apiBaseUrl"])
                );

            await builder.Build().RunAsync();
        }
    }
}
