using AutoFixture;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class CustomTypeDemos
    {
        [Test]
        public void ManualCreation()
        {
            //arrange
            var sut = new EmailMessageBuffer();

            EmailMessage message =
                new EmailMessage(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>())
                {
                    Subject = "Hi"
                };

            //act
            sut.Add(message);

            //arrange
            Assert.That(sut.Emails, Has.Exactly(1).Items);
        }

        [Test]
        public void AutoCreation()
        {
            //arrange
            var fixture = new Fixture();
            var sut = new EmailMessageBuffer();

            EmailMessage message = fixture.Create<EmailMessage>();

            //act
            sut.Add(message);

            //arrange
            Assert.That(sut.Emails, Has.Exactly(1).Items);
        }
    }
}
