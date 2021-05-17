using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class SequenceDemos
    {
        [Test]
        public void SequenceOfStrings()
        {
            //arrange
            var fixture = new Fixture();

            IEnumerable<string> messages = fixture.CreateMany<string>();

            //etc.

            Assert.Pass();
        }

        [Test]
        public void ExplicitNumberOfItems()
        {
            //arrange
            var fixture = new Fixture();

            IEnumerable<string> messages = fixture.CreateMany<string>(42);

            //etc.

            Assert.Pass();
        }

        [Test]
        public void AddingToExistingList()
        {
            //arrage
            var fixture = new Fixture();

            var sut = new DebugMessageBuffer();

            fixture.AddManyTo(sut.Messages, 10);

            //act
            sut.WriteMessages();

            //assert
            Assert.That(sut.MessagesWritten, Is.EqualTo(10));
        }

        [Test]
        public void AddingToExisitingListWithCreatorFunction()
        {
            //arrage
            var fixture = new Fixture();

            var sut = new DebugMessageBuffer();

            fixture.AddManyTo(sut.Messages, () => "Hi");

            //act
            sut.WriteMessages();

            //assert
            Assert.Pass();
        }
    }
}
