using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;

namespace AutoFixtureDemo.Tests
{
    public class EmailMessageBufferShould
    {
        private Fixture _fixture { get; set; }

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void AddMessageToBuffer()
        {
            var sut = new EmailMessageBuffer();

            sut.Add(_fixture.Create<EmailMessage>());

            Assert.That(sut.UnsentMessagesCount, Is.EqualTo(1));
        }

        [Test]
        public void RemoveMessageFromBufferWhenSent()
        {
            var sut = new EmailMessageBuffer();

            sut.Add(_fixture.Create<EmailMessage>());

            sut.SendAll();

            Assert.That(sut.UnsentMessagesCount, Is.EqualTo(0));
        }

        [Test]
        public void SendOnlySpecifiedNumberOfMessages()
        {
            var sut = new EmailMessageBuffer();

            sut.Add(_fixture.Create<EmailMessage>());
            sut.Add(_fixture.Create<EmailMessage>());
            sut.Add(_fixture.Create<EmailMessage>());

            sut.SendLimited(2);

            Assert.That(sut.UnsentMessagesCount, Is.EqualTo(1));
        }
    }
}
