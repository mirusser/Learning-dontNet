using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;

namespace AutoFixtureDemo.Tests
{
    public class CalculatorShould
    {

        [Test]
        [InlineAutoData]
        [InlineAutoData(0)]
        [InlineAutoData(-5)]
        public void Add(int a, int b, Calculator sut)
        {
            sut.Add(a);
            sut.Add(b);

            Assert.That(sut.Value, Is.EqualTo(a + b));
        }

        [Test]
        [AutoData]
        public void AddTwoPositiveNumbers(
            int a, 
            int b, 
            Calculator sut)
        {
            sut.Add(a);
            sut.Add(b);

            Assert.That(sut.Value, Is.EqualTo(a + b));
        }
    }
}
