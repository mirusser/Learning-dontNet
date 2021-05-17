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

        [Test]
        public void Decimals()
        {
            //arrange
            var fixture = new Fixture();
            var sut = new DecimalCalculator();

            var num = fixture.Create<decimal>();

            //act
            sut.Add(num);

            //assert
            Assert.That(num, Is.EqualTo(sut.Value));
        }
    }
}