using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using DeskBooker.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DeskBooker.Web.Tests.Pages
{
    public class BookDeskModelTests
    {
        private Mock<IDeskBookingRequestProcessor> _processorMock;
        private BookDeskModel _bookDeskModel;
        private DeskBookingResult _deskBookingResult;

        [SetUp]
        public void SetUp()
        {
            _processorMock = new Mock<IDeskBookingRequestProcessor>();

            _bookDeskModel = new BookDeskModel(_processorMock.Object)
            {
                DeskBookingRequest = new DeskBookingRequest()
            };

            _deskBookingResult = new DeskBookingResult
            {
                Code = DeskBookingResultCode.Success
            };

            _processorMock
                .Setup(x => x.BookDesk(_bookDeskModel.DeskBookingRequest))
                .Returns(_deskBookingResult);
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void ShouldCallBookDeskMethodOfProcessorIfModelIsValid(
            int expectedBookDeskCalls,
            bool isModelValid
            )
        {
            //arrange
            if (!isModelValid)
            {
                _bookDeskModel.ModelState.AddModelError("JustAKey", "AnErrorMessage");
            }

            //act
            _bookDeskModel.OnPost();

            //assert
            _processorMock.Verify(x => x.BookDesk(_bookDeskModel.DeskBookingRequest), Times.Exactly(expectedBookDeskCalls));
        }

        [Test]
        public void ShouldAddModelErrorIfNoDeskIsAvailable()
        {
            //arrange
            _deskBookingResult.Code = DeskBookingResultCode.NoDeskAvailable;

            //act
            _bookDeskModel.OnPost();

            //assert
            Assert.IsTrue(_bookDeskModel.ModelState.TryGetValue("DeskBookingRequest.Date", out ModelStateEntry modelStateEntry));
            Assert.AreEqual(1, modelStateEntry.Errors.Count);
            var modelError = modelStateEntry.Errors[0];
            Assert.AreEqual("No desk available for selected date", modelError.ErrorMessage);
        }

        [Test]
        public void ShouldNotAddModelErrorIfDeskIsAvailable()
        {
            //arrange
            _deskBookingResult.Code = DeskBookingResultCode.Success;

            //act
            _bookDeskModel.OnPost();

            //assert
            Assert.IsFalse(_bookDeskModel.ModelState.TryGetValue("DeskBookingRequest.Date", out ModelStateEntry modelStateEntry));
        }

        [Test]
        [TestCase(typeof(PageResult), false, null)]
        [TestCase(typeof(PageResult), true, DeskBookingResultCode.NoDeskAvailable)]
        [TestCase(typeof(RedirectToPageResult), true, DeskBookingResultCode.Success)]
        public void ShouldReturnExpectedActionResult(
            Type expectedActionResultType,
            bool isModelValid,
            DeskBookingResultCode? deskBookingResultCode)
        {
            // Arrange
            if (!isModelValid)
            {
                _bookDeskModel.ModelState.AddModelError("JustAKey", "AnErrorMessage");
            }

            if (deskBookingResultCode.HasValue)
            {
                _deskBookingResult.Code = deskBookingResultCode.Value;
            }

            // Act
            IActionResult actionResult = _bookDeskModel.OnPost();

            // Assert
            Assert.IsInstanceOf(expectedActionResultType, actionResult);
        }

        [Test]
        public void ShouldRedirectToBookDeskConfirmationPage()
        {
            //arrange
            _deskBookingResult.Code = DeskBookingResultCode.Success;
            _deskBookingResult.DeskBookingId = 42;
            _deskBookingResult.FirstName = "John";
            _deskBookingResult.Date = new DateTime(2021, 5, 5);

            //act
            IActionResult actionResult = _bookDeskModel.OnPost();

            //assert
            var redirectToPageResult = (RedirectToPageResult)actionResult;
            Assert.That(redirectToPageResult.PageName, Is.EqualTo("BookDeskConfirmation"));

            IDictionary<string, object> routeValues = redirectToPageResult.RouteValues;
            Assert.That(routeValues.Count, Is.EqualTo(3));

            Assert.That(routeValues.TryGetValue("DeskBookingId", out object deskBookingId), Is.True);
            Assert.That(_deskBookingResult.DeskBookingId, Is.EqualTo(deskBookingId));

            Assert.That(routeValues.TryGetValue("FirstName", out object firstName), Is.True);
            Assert.That(_deskBookingResult.FirstName, Is.EqualTo(firstName));

            Assert.That(routeValues.TryGetValue("Date", out object date), Is.True);
            Assert.That(_deskBookingResult.Date, Is.EqualTo(date));
        }
    }
}
