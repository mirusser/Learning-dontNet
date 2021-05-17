using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using NUnit;
using NUnit.Framework;

namespace AutoFixtureDemo.Tests
{
    public class StringDemos
    {
        [Test]
        public void BasicStrings()
        {
            //arrange
            var fixture = new Fixture();
            var sut = new NameJoiner();

            var firstName = fixture.Create("First_");
            var lastName = fixture.Create("Last_");

            //act
            var result = sut.Join(firstName, lastName);

            //assert
            Assert.That($"{firstName} {lastName}", Is.EqualTo(result));
        }
    }
}
