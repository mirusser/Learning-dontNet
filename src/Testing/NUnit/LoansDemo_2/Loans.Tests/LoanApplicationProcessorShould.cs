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

        delegate void ValidateCallback(
            string applicantName, 
            int applicantAge, 
            string applicantAddress, 
            ref IdentityVerificationStatus status);

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

            //Configuring mock object method return values
            mockIdentityVerifier
                .Setup(x => x.Validate(
                    "Sarah",
                    25,
                    "133 Pluralsight Drive, Draper, Utah"))
                .Returns(true);

            //Argument matching in mocked methods
            //mockIdentityVerifier
            //    .Setup(x => x.Validate(
            //        It.IsAny<string>(), 
            //        It.IsAny<int>(), 
            //        It.IsAny<string>()))
            //    .Returns(true);

            //Mocking methods with out parameter
            //bool isValidOutValue = true;
            //mockIdentityVerifier
            //    .Setup(x => x.Validate(
            //        "Sarah",
            //        25,
            //        "133 Pluralsight Drive, Draper, Utah",
            //        out isValidOutValue));

            //Mocking methods with ref parameter
            //mockIdentityVerifier
            //    .Setup(x => x.Validate(
            //        "Sarah",
            //        25,
            //        "133 Pluralsight Drive, Draper, Utah",
            //        ref It.Ref<IdentityVerificationStatus>.IsAny))
            //    .Callback(new ValidateCallback(
            //        (string applicantName,
            //        int applicantAge,
            //        string applicantAddress,
            //        ref IdentityVerificationStatus status) => status = new IdentityVerificationStatus(true)));

            //var mockScoreValue = new Mock<ScoreValue>();
            //mockScoreValue.Setup(x => x.Score).Returns(300);

            //var mockScoreResult = new Mock<ScoreResult>();
            //mockScoreResult.Setup(x => x.ScoreValue).Returns(mockScoreValue.Object);

            //var mockCreditScorer = new Mock<ICreditScorer>();
            //mockCreditScorer.Setup(x => x.ScoreResult).Returns(mockScoreResult.Object);

            //Setup a mock property to return a specific value
            //mockCreditScorer.Setup(x => x.Score).Returns(300);

            //auto mocking property hierarchy (caveat: property must be marked as 'virtual'
            //var mockCreditScorer = new Mock<ICreditScorer>();
            //mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);

            //automatically populate properites (also nested objects)
            //var mockCreditScorer = new Mock<ICreditScorer>()
            //{
            //    DefaultValue = DefaultValue.Mock
            //};

            var mockCreditScorer = new Mock<ICreditScorer>();

            //Tracks changes of properties
            //mockCreditScorer.SetupProperty(x => x.Count);
            mockCreditScorer.SetupAllProperties(); //overrides any specific setup, should be used first

            mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);

            //system under test
            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);

            sut.Process(application);

            //veryfing that getter was called
            mockCreditScorer.VerifyGet(x => x.ScoreResult.ScoreValue.Score, Times.Once);

            //veryfing that setter was called (with value that was set)
            mockCreditScorer.VerifySet(x => x.Count = 1, Times.Once);


            Assert.That(application.GetIsAccepted(), Is.True);
            Assert.That(mockCreditScorer.Object.Count, Is.EqualTo(1));
        }

        [Test]
        public void InitializeIdentity()
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

            mockIdentityVerifier
                .Setup(x => x.Validate(
                    "Sarah",
                    25,
                    "133 Pluralsight Drive, Draper, Utah"))
                .Returns(true);

            var mockCreditScorer = new Mock<ICreditScorer>();
            mockCreditScorer.SetupAllProperties();
            mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);

            //system under test
            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);

            sut.Process(application);

            //veryfing that method (without parameters) was called/exeucted at least once
            mockIdentityVerifier.Verify(x => x.Initialize());

            mockIdentityVerifier.Verify(x => x.Validate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            mockIdentityVerifier.VerifyNoOtherCalls();
        }

        [Test]
        public void CalculateCall()
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

            mockIdentityVerifier
                .Setup(x => x.Validate(
                    "Sarah",
                    25,
                    "133 Pluralsight Drive, Draper, Utah"))
                .Returns(true);

            var mockCreditScorer = new Mock<ICreditScorer>();
            mockCreditScorer.SetupAllProperties();
            mockCreditScorer.Setup(x => x.ScoreResult.ScoreValue.Score).Returns(300);

            //system under test
            var sut = new LoanApplicationProcessor(mockIdentityVerifier.Object, mockCreditScorer.Object);

            sut.Process(application);

            //veryfing that method (without parameters) was called/exeucted at least once
            //mockCreditScorer.Verify(x => x.CalculateScore(It.IsAny<string>(), It.IsAny<string>()));
            mockCreditScorer.Verify(x => x.CalculateScore("Sarah", "133 Pluralsight Drive, Draper, Utah"), times: Times.AtLeastOnce);
        }

        //Configuring mock methods to return null
        [Test]
        public void NullReturnExample()
        {
            var mock = new Mock<INullExample>();

            mock.Setup(x => x.SomeMethod()).Returns<string>(null);

            string mockReturnValue = mock.Object.SomeMethod();

            Assert.That(mockReturnValue, Is.Null);
        }
    }

    public interface INullExample
    {
        string SomeMethod();
    }
}
