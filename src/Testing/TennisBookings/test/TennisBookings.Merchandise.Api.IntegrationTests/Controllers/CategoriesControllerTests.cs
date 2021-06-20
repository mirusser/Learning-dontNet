using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using TennisBookings.Merchandise.Api.IntegrationTests.Models;
using TennisBookings.Merchandise.Api.IntegrationTests.TestHelper;

namespace TennisBookings.Merchandise.Api.IntegrationTests.Controllers
{
    public class CategoriesControllerTests
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _httpClient;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new();
            //_factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/categories");

            //_httpClient = _factory.CreateDefaultClient(new Uri("http://localhost/api/categories"));

            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost/api/categories")
            });
        }

        [Test]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            var response = await _httpClient.GetAsync("");

            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetAll_ReturnsExpectedMediaType()
        {
            var response = await _httpClient.GetAsync("");

            Assert.That(response.Content.Headers.ContentType.MediaType, Is.EqualTo("application/json"));
        }

        [Test]
        public async Task GetAll_ReturnsContent()
        {
            var response = await _httpClient.GetAsync("");

            Assert.That(response.Content, Is.Not.Null);
            Assert.That(response.Content.Headers.ContentLength, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetAll_ReturnsExpectedJson()
        {
            var expectedCategories = new List<string> { "Accessories", "Bags", "Balls", "Clothing", "Rackets" };
            var response = await _httpClient.GetStringAsync("");

            var model = JsonConvert.DeserializeObject<ExpectedCategoriesModel>(response);

            //the line below doesnt work fo some reason, im to lazy to deal with it right now
            //var model = await JsonSerializer.DeserializeAsync<ExpectedCategoriesModel>(responseStream, JsonSerializerHelper.DefaultSerialisationOptions); 

            using (new AssertionScope())
            {
                model.Should().NotBeNull();
                model.AllowedCategories.Should().NotBeNull();
                model?.AllowedCategories.OrderBy(s => s).Should().BeEquivalentTo(expectedCategories.OrderBy(s => s));
            }
        }

        [Test]
        public async Task GetAll_ReturnsExpectedResponse()
        {
            var expectedCategories = new List<string> { "Accessories", "Bags", "Balls", "Clothing", "Rackets" };

            var model = await _httpClient.GetFromJsonAsync<ExpectedCategoriesModel>("");

            using (new AssertionScope())
            {
                model.Should().NotBeNull();
                model.AllowedCategories.Should().NotBeNull();
                model?.AllowedCategories.OrderBy(s => s).Should().BeEquivalentTo(expectedCategories.OrderBy(s => s));
            }
        }

        [Test]
        public async Task GetAll_SetsExpectedCacheControlHeader()
        {
            var response = await _httpClient.GetAsync("");

            var header = response.Headers.CacheControl;

            using (new AssertionScope())
            {
                header.MaxAge.HasValue.Should().BeTrue();
                header.MaxAge.Should().Be(TimeSpan.FromMinutes(5));
                header.Public.Should().BeTrue();
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _httpClient.Dispose();
            _factory.Dispose();
        }
    }
}
