using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace TennisBookings.Merchandise.Api.IntegrationTests
{
    public class HealthCheckTests 
    {
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            WebApplicationFactory<Startup> factory = new();

            _httpClient = factory.CreateDefaultClient();
        }

        [Test]
        public async Task HealthCheckReturnsOk()
        {
            var response = await _httpClient.GetAsync("/health");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //response.EnsureSuccessStatusCode();
        }
    }
}
