using DeskBooker.Core.Domain;
using NUnit.Framework;
using System;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
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

            var processor = new DeskBookingRequestProcessor();

            //act
            DeskBookingResult result = processor.BookDesk(request);

            //assert
            Assert.That(result, Is.Not.Null);
            Assert.That(request.FirstName, Is.EqualTo(result.FirstName));
            Assert.That(request.LastName, Is.EqualTo(result.LastName));
            Assert.That(request.Email, Is.EqualTo(result.Email));
            Assert.That(request.Date, Is.EqualTo(result.Date));
        }
    }
}
