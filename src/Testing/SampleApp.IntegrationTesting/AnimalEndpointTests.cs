using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SampleApp.Models;
using SampleApp.Services;
using Xunit;

namespace SampleApp.IntegrationTesting
{
    public class AnimalEndpointTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public AnimalEndpointTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            //_webApplicationFactory = webApplicationFactory; //to use actual service (AnimalService) which is registered in Startup

            //to mock service:
            _webApplicationFactory = webApplicationFactory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IAnimalService, AnimalServiceMock>(); //you could use Moq here
                });
            });
        }

        public class AnimalServiceMock : IAnimalService
        {
            public Animal GetAnimal()
            {
                return new()
                {
                    Id = 2,
                    Name = "Foo2",
                    Type = "Bar2",
                };
            }
        }

        [Fact]
        public async Task GetsAnimal()
        {
            var client = _webApplicationFactory.CreateClient();

            //var response = await client.GetAsync("/api/animals");
            //var content = await response.Content.ReadAsStringAsync();
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var animal = await client.GetFromJsonAsync<Animal>("/api/animals");

            Assert.NotNull(animal);
            Assert.Equal(2, animal.Id);
        }
    }
}