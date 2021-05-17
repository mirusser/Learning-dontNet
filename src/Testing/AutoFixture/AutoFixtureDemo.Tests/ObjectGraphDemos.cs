using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class ObjectGraphDemos
    {
        [Test]
        public void AutoCreation()
        {
            //arrange
            var fixture = new Fixture();

            Order order = fixture.Create<Order>();

            //act and assert phases

            Assert.Pass();
        }
    }
}
