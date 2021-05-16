using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private Mock<IDeskRepository> _deskRepositoryMock;

        private DeskBookingRequestProcessor _processor;

        private DeskBookingRequest _request;
        private List<Desk> _availableDesks;

        [SetUp]
        public void SetUp()
        {
            _request = new DeskBookingRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@email.com",
                Date = new DateTime(2021, 5, 5)
            };

            _availableDesks = new List<Desk>() { new Desk() { Id = 42 } };

            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();

            _deskRepositoryMock = new Mock<IDeskRepository>();
            _deskRepositoryMock
                .Setup(x => x.GetAvailableDesks(_request.Date))
                .Returns(_availableDesks);

            _processor = new DeskBookingRequestProcessor(
                _deskBookingRepositoryMock.Object,
                _deskRepositoryMock.Object);
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
            Assert.That(_availableDesks.First().Id, Is.EqualTo(savedDeskBooking.DeskId));
        }

        [Test]
        public void ShouldNotSaveDeskBookingIfNoDeskIsAvailable()
        {
            _availableDesks.Clear();

            _processor.BookDesk(_request);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Never);
        }

        [Test]
        [TestCase(DeskBookingResultCode.Success, true)]
        [TestCase(DeskBookingResultCode.NoDeskAvailable, false)]
        public void ShouldReturnExpectedResultCode(
            DeskBookingResultCode expectedResultCode,
            bool isDeskAvailable)
        {
            if (!isDeskAvailable)
            {
                _availableDesks.Clear();
            }

            var result = _processor.BookDesk(_request);

            Assert.That(expectedResultCode, Is.EqualTo(result.Code));
        }

        [Test]
        [TestCase(7, true)]
        [TestCase(null, false)]
        public void ShouldReturnExpectedDeskBookingId(
            int? expectedDeskBookingId,
            bool isDeskAvailable)
        {
            if (!isDeskAvailable)
            {
                _availableDesks.Clear();
            }
            else
            {
                _deskBookingRepositoryMock
                    .Setup(x => x.Save(It.IsAny<DeskBooking>()))
                    .Callback<DeskBooking>(deskBooking => 
                    {
                        deskBooking.Id = expectedDeskBookingId.Value;
                    });
            }

            var result = _processor.BookDesk(_request);

            Assert.That(expectedDeskBookingId, Is.EqualTo(result.DeskBookingId));
        }

    }
}
