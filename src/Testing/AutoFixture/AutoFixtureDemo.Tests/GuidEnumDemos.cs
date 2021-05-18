using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class GuidEnumDemos
    {
        [Test]
        public void Guid()
        {
            var fixture = new Fixture();
            var sut =
                new EmailMessage(
                    fixture.Create<string>(),
                    fixture.Create<string>(),
                    fixture.Create<bool>(),
                    fixture.Create<string>())
                {
                    Id = fixture.Create<Guid>()
                };

            Assert.Pass();
        }
    }
}
