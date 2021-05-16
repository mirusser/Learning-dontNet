using NUnit.Framework;
using AutoFixture;

namespace AutoFixtureDemo.Tests
{
    public class NumberDemos
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Ints()
        {
            //arrange
            var sut = new IntCalculator();
            var fixture = new Fixture();

            //act
            sut.Subtract(fixture.Create<int>());

            Assert.That(sut.Value, Is.LessThan(0));
        }
    }
}