using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class BookingShould
    {
        [Test]
        public void CalculateTotalFlightTime()
        {
            // arrange
            var fixture = new Fixture();
            fixture.Inject(new AirportCode("LHR"));

            var sut = fixture.Create<Booking>();

            // etc.
        }

    }
}
