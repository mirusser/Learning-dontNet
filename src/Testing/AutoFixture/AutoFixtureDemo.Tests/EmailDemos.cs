using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AutoFixtureDemo.Tests
{
    public class EmailDemos
    {
        [Test]
        public void Email()
        {
            //arrange
            var fixture = new Fixture();

            //var localPart = fixture.Create<EmailAddressLocalPart>().LocalPart;
            //var domain = fixture.Create<DomainName>().Domain;
            //var fullAddress = $"{localPart}@{domain}";

            var email = fixture.Create<MailAddress>();

            var sut =
                new EmailMessage(
                    email.Address,
                    fixture.Create<string>(),
                    fixture.Create<bool>(),
                    fixture.Create<string>())
                {
                    Id = fixture.Create<Guid>()
                };

            Assert.Pass();
        }
    }
}
