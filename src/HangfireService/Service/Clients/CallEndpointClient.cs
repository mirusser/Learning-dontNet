using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HangfireService.Clients
{
    public class CallEndpointClient : ICallEndpointClient
    {
        private readonly HttpClient _httpClient;

        public CallEndpointClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task GetMethod(string url, CancellationToken cancellation = default)
        {
            await _httpClient.GetAsync(url, cancellation).ConfigureAwait(false);
        }
    }
}
