using Loans.Domain.Applications.Values;
using NUnit.Framework;
using FluentAssertions;

namespace Loans.Tests
{
    public class LoanAmountShould
    {
        [Test]
        public void StoreCurrencyCode()
        {
            var loanAmount = new LoanAmount("USD", 100_000);

            //Assert.That(sut.CurrencyCode, Is.EqualTo("USD"));

            //loanAmount.CurrencyCode.Should().BeEquivalentTo("USD"); // case insensitive
            loanAmount.CurrencyCode.Should().Be("USD");
            loanAmount.CurrencyCode.Should().BeOneOf("USD", "AUD", "GBP");
            loanAmount.CurrencyCode.Should().Contain("S"); //case sensitive
            loanAmount.CurrencyCode.Should().StartWith("U");
            loanAmount.CurrencyCode.Should().EndWith("D");
            loanAmount.CurrencyCode.Should().Match("*D");
            loanAmount.CurrencyCode.Should().Match("*S*");
            loanAmount.CurrencyCode.Should().MatchRegex("[A-Z]{3}");
        }
    }
}
