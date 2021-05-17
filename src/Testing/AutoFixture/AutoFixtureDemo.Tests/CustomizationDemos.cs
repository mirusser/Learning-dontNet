using AutoFixture;
using AutoFixtureDemo.Tests.Generators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class CustomizationDemos
    {
        [Test]
        public void DateTimeCustomization()
        {
            //arrange
            var fixture = new Fixture();

            //fixture.Customize(new CurrentDateTimeCustomization());
            fixture.Customizations.Add(new CurrentDateTimeGenerator());

            var date1 = fixture.Create<DateTime>();
            var date2 = fixture.Create<DateTime>();

            //etc.

            Assert.Pass();
        }

        [Test]
        public void CustomizedPipeline()
        {
            //arrange
            var fixture = new Fixture();
            fixture.Customizations.Add(new AirportCodeStringPropertyGenerator());

            var flight = fixture.Create<FlightDetails>();
            var airport = fixture.Create<Airport>();

            //etc.

            Assert.Pass();
        }
    }
}
