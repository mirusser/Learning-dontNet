using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SampleApp.IntegrationTesting
{
    public class FileEndpointTests : IClassFixture<FileTestingFixture>
    {
        private readonly FileTestingFixture _fileTestingFixture;

        public FileEndpointTests(FileTestingFixture webApplicationFactory)
        {
            _fileTestingFixture = webApplicationFactory;
        }

        [Fact]
        public async Task Should_SaveFileToDisk()
        {
            var client = _fileTestingFixture.CreateClient();

            MultipartFormDataContent form = new();
            form.Add(new StreamContent(_fileTestingFixture.TestFile), "file", "base.png");
            var response = await client.PostAsync("/api/files", form);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var fileResponse = await client.GetAsync("/test_images/base.png");
            Assert.Equal(HttpStatusCode.OK, fileResponse.StatusCode);
        }
    }
}