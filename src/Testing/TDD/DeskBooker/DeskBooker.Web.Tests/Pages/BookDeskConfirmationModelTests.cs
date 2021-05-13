using DeskBooker.Web.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskBooker.Web.Tests.Pages
{
    public class BookDeskConfirmationModelTests
    {
        [Test]
        public void ShoulStoreParameterValuesInProperties()
        {
            //arrange
            int deskBookingId = 42;
            string firstName = "John";
            var date = new DateTime(2021, 5, 5);

            var bookDeskConfirmationModel = new BookDeskConfirmationModel();

            //act
            bookDeskConfirmationModel.OnGet(deskBookingId, firstName, date);

            //assert
            Assert.That(deskBookingId, Is.EqualTo(bookDeskConfirmationModel.DeskBookingId));
            Assert.That(firstName, Is.EqualTo(bookDeskConfirmationModel.FirstName));
            Assert.That(date, Is.EqualTo(bookDeskConfirmationModel.Date));
        }
    }
}
