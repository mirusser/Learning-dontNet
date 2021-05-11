using DeskBooker.Core.Domain;
using NUnit.Framework;
using System;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private DeskBookingRequestProcessor _processor;

        [SetUp]
        public void SetUp()
        {
            _processor = new DeskBookingRequestProcessor();
        }

        [Test]
        public void ShouldReturnDeskBookingResultWithRequestValues()
        {
            //arrange
            var request = new DeskBookingRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@email.com",
                Date = new DateTime(2021, 5, 5)
            };

            //act
            DeskBookingResult result = _processor.BookDesk(request);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(request.FirstName, Is.EqualTo(result.FirstName));
            Assert.That(request.LastName, Is.EqualTo(result.LastName));
            Assert.That(request.Email, Is.EqualTo(result.Email));
            Assert.That(request.Date, Is.EqualTo(result.Date));
        }

        [Test]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.That("request", Is.EqualTo(exception.ParamName));
        }
    }
}
