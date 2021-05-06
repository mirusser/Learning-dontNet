using Loans.Domain.Applications;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loans.Tests
{
    [TestFixture]
    public class ProductComparerShould
    {
        [Test]
        public void ReturnCorrectNumberOfComparisons()
        {
            var products = new List<LoanProduct>
            {
                new LoanProduct(1, "a", 1),
                new LoanProduct(2, "b", 2),
                new LoanProduct(3, "c", 3)
            };

            var sut = new ProductComparer(new LoanAmount("US", 200_000m), products);

            List<MonthlyRepaymentComparison> comparisions = 
                sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisions, Has.Exactly(3).Items);
        }

        [Test]
        public void NotReturnDuplicateComparisons()
        {
            var products = new List<LoanProduct>
            {
                new LoanProduct(1, "a", 1),
                new LoanProduct(2, "b", 2),
                new LoanProduct(3, "c", 3)
            };

            var sut = new ProductComparer(new LoanAmount("US", 200_000m), products);

            List<MonthlyRepaymentComparison> comparisions =
                sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisions, Is.Unique);
        }

        [Test]
        public void ReturnComparisonForFirstProduct()
        {
            var products = new List<LoanProduct>
            {
                new LoanProduct(1, "a", 1),
                new LoanProduct(2, "b", 2),
                new LoanProduct(3, "c", 3)
            };

            var sut = new ProductComparer(new LoanAmount("US", 200_000m), products);

            //Need to also to know the expected monthly repayment
            List<MonthlyRepaymentComparison> comparisions =
                sut.CompareMonthlyRepayments(new LoanTerm(30));

            var expectedProduct = new MonthlyRepaymentComparison("a", 1, 643.28m);

            Assert.That(comparisions, Does.Contain(expectedProduct));
        }

        [Test]
        public void ReturnComparisonForFirstProductWithPartialKnownExpectedValues()
        {
            var products = new List<LoanProduct>
            {
                new LoanProduct(1, "a", 1),
                new LoanProduct(2, "b", 2),
                new LoanProduct(3, "c", 3)
            };

            var sut = new ProductComparer(new LoanAmount("US", 200_000m), products);

            List<MonthlyRepaymentComparison> comparisions =
                sut.CompareMonthlyRepayments(new LoanTerm(30));

            //Don't care about the expected monthly repayment,
            //only thtat the product is there

            //Assert.That(comparisions, Has.Exactly(1)
            //                                .Property("ProductName")
            //                                .EqualTo("a")
            //                                .And
            //                                .Property("InterestRate")
            //                                .EqualTo(1)
            //                                .And
            //                                .Property("MonthlyRepayment")
            //                                .GreaterThan(0));

            Assert.That(comparisions, Has.Exactly(1)
                                        .Matches<MonthlyRepaymentComparison>(
                                        item => 
                                            item.ProductName == "a" && 
                                            item.InterestRate == 1 && 
                                            item.MonthlyRepayment > 0));
        }
    }
}
