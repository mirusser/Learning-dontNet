using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskBooker.Core.Validation
{
    public class DateInFutureAttributeTests
    {
        [TestCase(false, -1)]
        [TestCase(false, 0)]
        [TestCase(true, 1)]
        public void ShouldValidateDateIsInFuture(bool expectedIsValid, int secondsToAdd)
        {
            var dateTimeNow = new DateTime(2021, 5, 5);

            var attribute = new DateInFutureAttribute(() => dateTimeNow);

            var isValid = attribute.IsValid(dateTimeNow.AddSeconds(secondsToAdd));

            Assert.That(expectedIsValid, Is.EqualTo(isValid));
        }

        [Test]
        public void ShouldHaveExpectedErrorMessage()
        {
            var attribute = new DateInFutureAttribute();

            Assert.That("Date must be in the future", Is.EqualTo(attribute.ErrorMessage));
        }

    }
}
