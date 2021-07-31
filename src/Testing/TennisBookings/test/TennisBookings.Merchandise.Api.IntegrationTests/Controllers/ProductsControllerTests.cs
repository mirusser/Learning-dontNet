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

namespace TennisBookings.Merchandise.Api.IntegrationTests.Controllers
{
    public class ProductsControllerTests
    {
        private CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new();
            _factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/products/");
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GetAll_ReturnsExpectedArrayOfProducts()
        {
            _factory.FakeCloudDatabase.ResetDefaultProducts(useCustomIfAvailable: false);

            var products = await _client.GetFromJsonAsync<ExpectedProductModel[]>("");

            Assert.That(products, Is.Not.Null);
            Assert.That(products.Count(), Is.EqualTo(_factory.FakeCloudDatabase.Products.Count));
        }

        [Test]
        public async Task Get_ReturnsExpectedProduct()
        {
            var firstProduct = _factory.FakeCloudDatabase.Products.First();
            var product = await _client.GetFromJsonAsync<ExpectedProductModel>($"{firstProduct.Id}");

            Assert.That(product, Is.Not.Null);
            Assert.That(product.Name, Is.EqualTo(firstProduct.Name));
        }
    }
}
