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
        WebApplicationFactory<Startup> _factory;
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new();

            _httpClient = _factory.CreateDefaultClient();
        }

        [Test]
        public async Task HealthCheckReturnsOk()
        {
            var response = await _httpClient.GetAsync("/health");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //response.EnsureSuccessStatusCode();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _httpClient.Dispose();
            _factory.Dispose();
        }
    }
}
