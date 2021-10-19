using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using xUnitTesting.Mocking.Units;

namespace xUnitTesting.Mocking
{
    public class IntranetCommunicationsTest
    {
        public class MockHttpHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage
                {
                    Content = new StringContent("[\"foo\", \"bar\", \"baz\"]")
                });
            }
        }

        [Fact]
        public async Task Should_FetchesNames()
        {
            var client = new HttpClient(new MockHttpHandler())
            {
                BaseAddress = new("http://example.com")
            };
            var iComm = new IntranetCommunications(client);

            var names = (await iComm.FetchNames()).ToList();

            names.Count.Should().Equals(3);
            names.Should().Contain(new string[] { "foo", "bar", "baz" });
        }
    }
}