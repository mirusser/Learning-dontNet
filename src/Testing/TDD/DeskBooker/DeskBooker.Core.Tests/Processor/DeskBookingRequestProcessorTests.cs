using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;
using Moq;
using NUnit.Framework;
using System;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private DeskBookingRequestProcessor _processor;
        private DeskBookingRequest _request;

        [SetUp]
        public void SetUp()
        {
            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();

            _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object);

            _request = new DeskBookingRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@email.com",
                Date = new DateTime(2021, 5, 5)
            };
        }

        [Test]
        public void ShouldReturnDeskBookingResultWithRequestValues()
        {
            //act
            DeskBookingResult result = _processor.BookDesk(_request);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(_request.FirstName, Is.EqualTo(result.FirstName));
            Assert.That(_request.LastName, Is.EqualTo(result.LastName));
            Assert.That(_request.Email, Is.EqualTo(result.Email));
            Assert.That(_request.Date, Is.EqualTo(result.Date));
        }

        [Test]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.That("request", Is.EqualTo(exception.ParamName));
        }

        [Test]
        public void ShouldSaveDeskBooking()
        {
            DeskBooking savedDeskBooking = null;

            _deskBookingRepositoryMock
                .Setup(x =>  x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(deskBooking => 
                {
                    savedDeskBooking = deskBooking;
                });

            _processor.BookDesk(_request);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);

            Assert.That(savedDeskBooking, Is.Not.Null);
            Assert.That(_request.FirstName, Is.EqualTo(savedDeskBooking.FirstName));
            Assert.That(_request.LastName, Is.EqualTo(savedDeskBooking.LastName));
            Assert.That(_request.Email, Is.EqualTo(savedDeskBooking.Email));
            Assert.That(_request.Date, Is.EqualTo(savedDeskBooking.Date));
        }
    }
}
