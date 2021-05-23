using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TennisBookings.Merchandise.Api.Data.Dto;
using TennisBookings.Merchandise.Api.External.Database;
using TennisBookings.Merchandise.Api.IntegrationTests.Fakes;
using TennisBookings.Merchandise.Api.IntegrationTests.Models;

namespace TennisBookings.Merchandise.Api.IntegrationTests.Controllers
{
    public class StockControllerTests
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new();
            _factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/stock/");
            _client = _factory.CreateClient();

            //_factory = new();
            ////_factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/categories");

            //_client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            //{
            //    BaseAddress = new Uri("http://localhost/api/categories")
            //});
        }

        #region Old ways of testing this endpoint
        [Test]
        public async Task GetStockTotal_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("total");

            response.EnsureSuccessStatusCode();
        }

        //[Test]
        //public async Task GetStockTotal_ReturnsExpecedJsonContentString()
        //{
        //    var response = await _client.GetStringAsync("total");

        //    Assert.That(response, Is.EqualTo("{\"stockItemTotal\":100}"));
        //}

        [Test]
        public async Task GetStockTotal_ReturnsExpectedJsonContentType()
        {
            var response = await _client.GetAsync("total");

            Assert.That(response.Content.Headers.ContentType.MediaType, Is.EqualTo("application/json"));
        }
        #endregion

        [Test]
        public async Task GetStockTotal_ReturnsExpectedJson()
        {
            var response = await _client.GetFromJsonAsync<ExpectedStockTotalOutputModel>("total");

            Assert.That(response, Is.Not.Null);
            Assert.That(response.StockItemTotal, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetStockTotal_ReturnsExpectedStockQuantity()
        {
            var cloudDatabse = new FakeCloudDatabase(new[]
            {
                new ProductDto{ StockCount = 200},
                new ProductDto{ StockCount = 500},
                new ProductDto{ StockCount = 300},
            });

            var client = _factory.WithWebHostBuilder(builder => 
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<ICloudDatabase>(cloudDatabse);
                });
            }).CreateClient();

            var response = await client.GetFromJsonAsync<ExpectedStockTotalOutputModel>("total");

            Assert.That(response.StockItemTotal, Is.EqualTo(1000));
        }
    }
}
