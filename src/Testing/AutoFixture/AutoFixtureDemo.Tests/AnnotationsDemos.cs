using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class AnnotationsDemos
    {
        [Test]
        public void BasicStrings()
        {
            //arrange
            var fixture = new Fixture();

            var player = fixture.Create<PlayerCharacter>();

            //act and assert phases...

            Assert.Pass();
        }
    }
}
