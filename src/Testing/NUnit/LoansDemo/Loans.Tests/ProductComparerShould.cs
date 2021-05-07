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
    [Category("Product Comparison")]
    public class ProductComparerShould
    {
        private List<LoanProduct> _products;
        private ProductComparer _sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //Simulate long setup init time for this list of products
            //We assume that this list will not be modified by any tests
            //as this will potentially break other tests (i.e. break test isolation)
            _products = new List<LoanProduct>
            {
                new LoanProduct(1, "a", 1),
                new LoanProduct(2, "b", 2),
                new LoanProduct(3, "c", 3)
            };
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //Run after last test in this text class (fixture) executes
            //e.g. disposing of shared expensive setup performed in OneTimeSetUp

            //_products.Dispose(); e.g. if products implemented IDisposable
        }

        [SetUp]
        public void Setup()
        {
            _sut = new ProductComparer(new LoanAmount("US", 200_000m), _products);
        }

        [TearDown]
        public void TearDown()
        {
            //Runs after each test executes
            //for example: sut.Dispose();
        }

        [Test]
        //[Category("Product Comparison")]
        public void ReturnCorrectNumberOfComparisons()
        {
            List<MonthlyRepaymentComparison> comparisions =
                _sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisions, Has.Exactly(3).Items);
        }

        [Test]
        public void NotReturnDuplicateComparisons()
        {
            List<MonthlyRepaymentComparison> comparisions =
                _sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisions, Is.Unique);
        }

        [Test]
        public void ReturnComparisonForFirstProduct()
        {
            //Need to also to know the expected monthly repayment
            List<MonthlyRepaymentComparison> comparisions =
                _sut.CompareMonthlyRepayments(new LoanTerm(30));

            var expectedProduct = new MonthlyRepaymentComparison("a", 1, 643.28m);

            Assert.That(comparisions, Does.Contain(expectedProduct));
        }

        [Test]
        public void ReturnComparisonForFirstProductWithPartialKnownExpectedValues()
        {
            List<MonthlyRepaymentComparison> comparisions =
                _sut.CompareMonthlyRepayments(new LoanTerm(30));

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
