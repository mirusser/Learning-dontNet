using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class CustomizeFixtureDemos
    {
        [Test]
        public void Error()
        {
            //arrange
            var fixture = new Fixture();

            fixture.Inject("LHR");

            var flight = fixture.Create<FlightDetails>();

            //etc.

            Assert.Pass();
        }

        [Test]
        public void SettingValueForCustomType()
        {
            //arrange
            var fixture = new Fixture();

            fixture.Inject(new FlightDetails() 
            {
                DepartureAirportCode = "PER",
                ArrivalAirportCode = "LHR",
                FlightDuration = TimeSpan.FromHours(10),
                AirlineName = "Awesome Aero"
            });

            var flight1 = fixture.Create<FlightDetails>();
            var flight2 = fixture.Create<FlightDetails>();

            //etc.

            Assert.Pass();
        }

        [Test]
        public void CustomCreationFunction()
        {
            //arrange
            var fixture = new Fixture();

            fixture.Register(() => DateTime.Now.Ticks.ToString());

            var string1 = fixture.Create<string>();
            var string2 = fixture.Create<string>();

            //etc.

            Assert.Pass();
        }

        [Test]
        public void FreezingValues()
        {
            var fixture = new Fixture();
            var id = fixture.Freeze<int>();
            var customerName = fixture.Freeze<string>();

            var sut = fixture.Create<Order>();

            Assert.That(sut.ToString(), Is.EqualTo($"{id}-{customerName}")); ;
        }

        [Test]
        public void OmitSettingSpecificProperties()
        {
            //arrange
            var fixture = new Fixture();

            var flight = 
                fixture
                .Build<FlightDetails>()
                .Without(x => x.ArrivalAirportCode)
                .Without(x => x.DepartureAirportCode)
                .Create();

            //etc.

            Assert.Pass();
        }

        [Test]
        public void OmitSettingAllProperties()
        {
            var fixture = new Fixture();

            var flight =
                fixture
                .Build<FlightDetails>()
                .OmitAutoProperties()
                .Create();

            Assert.Pass();
        }

        [Test]
        public void CustomixedBuilding()
        {
            var fixture = new Fixture();

            var flight =
                fixture
                .Build<FlightDetails>()
                .With(x => x.ArrivalAirportCode, "LAX")
                .With(x => x.DepartureAirportCode, "LHR")
                .Create();

            Assert.Pass();
        }

        [Test]
        public void CustomixedBuildingWithActions()
        {
            var fixture = new Fixture();

            var flight =
                fixture
                .Build<FlightDetails>()
                .With(x => x.ArrivalAirportCode, "LAX")
                .With(x => x.DepartureAirportCode, "LHR")
                .Without(x => x.MealOptions)
                .Do(x => x.MealOptions.Add("Chicken"))
                .Do(x => x.MealOptions.Add("Fish"))
                .Create();

            Assert.Pass();
        }

        [Test]
        public void CustomixedBuildingForAllTypesInFixture()
        {
            var fixture = new Fixture();

            fixture.Customize<FlightDetails>(f =>
                f.With(x => x.ArrivalAirportCode, "LAX")
                .With(x => x.DepartureAirportCode, "LHR")
                .Without(x => x.MealOptions)
                .Do(x => x.MealOptions.Add("Chicken"))
                .Do(x => x.MealOptions.Add("Fish")));

            var flight1 = fixture.Create<FlightDetails>();
            var flight2 = fixture.Create<FlightDetails>();

            Assert.Pass();
        }
    }
}
