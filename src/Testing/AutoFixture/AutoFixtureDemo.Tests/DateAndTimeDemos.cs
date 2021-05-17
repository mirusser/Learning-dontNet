using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class DateAndTimeDemos
    {
        [Test]
        public void DateTime()
        {
            //arrange
            var fixture = new Fixture();
            var logTime = fixture.Create<DateTime>();

            //act
            var result = LogMessageCreator.Create(fixture.Create<string>(), logTime);

            //assert
            Assert.That(result.Year, Is.EqualTo(logTime.Year));
        }
    }
}
