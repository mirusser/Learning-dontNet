using DeskBooker.Core.DataInterfaces;
using DeskBooker.Core.Domain;
using DeskBooker.Web.Pages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskBooker.Web.Tests.Pages
{
    public class DesksModelTests
    {
        [Test]
        public void ShouldGetAllDesks()
        {
            //arrange
            var desks = new List<Desk>() 
            { 
                new Desk(),
                new Desk(),
                new Desk()
            };

            var deskRepositoryMock = new Mock<IDeskRepository>();
            deskRepositoryMock
                .Setup(x => x.GetAll())
                .Returns(desks);

            var desksModel = new DesksModel(deskRepositoryMock.Object);

            //act
            desksModel.OnGet();

            //assert
            Assert.That(desks, Is.EqualTo(desksModel.Desks));

        }
    }
}
