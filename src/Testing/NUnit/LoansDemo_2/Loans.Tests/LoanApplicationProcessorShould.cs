using System;
using System.Collections.Generic;
using System.Text;
using Loans.Domain.Applications;
using NUnit.Framework;
using Moq;

namespace Loans.Tests
{
    public class LoanApplicationProcessorShould
    {
        [Test]
        public void DeclineLowSalary()
        {
            LoanProduct product = new(99, "Loan", 5.25m);
            LoanAmount amount = new("USD", 200_000);
            LoanApplication application =
                new(42,
                    product, 
                    amount,
                    "Sarah",
                    25,
                    "133 Pluralsight Drive, Draper, Utah",
                    64_999);

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            var mockCreditScorer = new Mock<ICreditScorer>();

            //system under test
            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);

            sut.Process(application);

            Assert.That(application.GetIsAccepted(), Is.False);
        }

        [Test]
        public void Accept()
        {
            LoanProduct product = new(99, "Loan", 5.25m);
            LoanAmount amount = new("USD", 200_000);
            LoanApplication application =
                new(42,
                    product,
                    amount,
                    "Sarah",
                    25,
                    "133 Pluralsight Drive, Draper, Utah",
                    65_000);

            var mockIdentityVerifier = new Mock<IIdentityVerifier>();
            var mockCreditScorer = new Mock<ICreditScorer>();

            //system under test
            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);

            sut.Process(application);

            Assert.That(application.GetIsAccepted(), Is.True);
        }
    }
}
