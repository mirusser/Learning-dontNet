using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace AutoFixtureDemo.Tests
{
    public class EmailMessageBufferShould2
    {
        [Test]
        public void SendEmailToGateway_Manual_Moq()
        {
            //arrange
            var fixture = new Fixture();

            var mockGateway = new Mock<IEmailGateway>();

            var sut = new EmailMessageBuffer2(mockGateway.Object);

            sut.Add(fixture.Create<EmailMessage>());

            //act
            sut.SendAll();

            //assert
            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());

        }

        [Test]
        [TestCase]
        public void SendEmailToGateway_AutoMoq()
        {
            //arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            var mockGateway = fixture.Freeze<Mock<IEmailGateway>>();

            var sut = fixture.Create<EmailMessageBuffer2>();

            sut.Add(fixture.Create<EmailMessage>());

            //act
            sut.SendAll();

            //assert
            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());
        }

        [Test]
        [AutoMoqData]
        public void SendEmailToGateway_AutoMoq2(
            EmailMessage message,
            [Frozen] Mock<IEmailGateway> mockGateway,
            EmailMessageBuffer2 sut)
        {
            //arrange
            sut.Add(message);

            //act
            sut.SendAll();

            //assert
            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());
        }
    }
}
