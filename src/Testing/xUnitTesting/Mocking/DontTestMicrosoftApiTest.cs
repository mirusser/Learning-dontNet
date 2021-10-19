using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using xUnitTesting.Mocking.Units;
using static xUnitTesting.Mocking.Units.DontTestMicrosoftApi;

namespace xUnitTesting.Mocking
{
    public class DontTestMicrosoftApiTest
    {
        private readonly Mock<IFiles> _fileMock = new();

        [Fact]
        public async Task Should_WritesToFileStream()
        {
            var memoryStream = new MemoryStream();
            _fileMock
                .Setup(x => x.OpenWriteStreamTo("path"))
                .Returns(memoryStream);
            var service = new DontTestMicrosoftApi(_fileMock.Object);

            await service.SaveFile("path", new MemoryStream(new byte[] { 2, 3, 4, 5 }));

            memoryStream.Length.Should().Equals(4);
        }
    }
}