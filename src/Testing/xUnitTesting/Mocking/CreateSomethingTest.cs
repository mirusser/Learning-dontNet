using System;
using FluentAssertions;
using Moq;
using Xunit;
using xUnitTesting.Mocking.Units;

namespace xUnitTesting.Mocking
{
    public class CreateSomethingTest
    {
        //You would have to create your own mocks when you are not using Moq
        //something like this (which could be pretty tiresome in a long run):

        //public class StoreMock : CreateSomething.IStore
        //{
        //    public int SaveAttempts { get; set; }
        //    public bool SaveResults { get; set; }
        //    public CreateSomething.Something LastSavedObject { get; set; }

        //    public bool Save(CreateSomething.Something something)
        //    {
        //        SaveAttempts++;
        //        LastSavedObject = something;
        //        return SaveResults;
        //    }
        //}

        private readonly Mock<CreateSomething.IStore> _storeMock = new();

        [Fact]
        public void Should_DoesntSaveToDatabaseWhenInvalidSomething()
        {
            CreateSomething createSomething = new(_storeMock.Object);

            var createSomethingResult = createSomething.Create(null);

            createSomethingResult.Success.Should().BeFalse();
            _storeMock.Verify(x => x.Save(It.IsAny<CreateSomething.Something>()), Times.Never);
        }

        [Fact]
        public void Should_SaveSomethingToDatabaseWhenValidSomething()
        {
            var something = new CreateSomething.Something()
            {
                Name = "foo"
            };
            _storeMock.Setup(x => x.Save(something)).Returns(true);
            CreateSomething createSomething = new(_storeMock.Object);

            var createSomethingResult = createSomething.Create(something);

            createSomethingResult.Success.Should().BeTrue();
            _storeMock.Verify(x => x.Save(something), Times.Once);
        }
    }
}