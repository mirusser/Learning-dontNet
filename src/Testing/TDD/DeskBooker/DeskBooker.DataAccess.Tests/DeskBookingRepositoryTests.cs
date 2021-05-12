using DeskBooker.Core.Domain;
using DeskBooker.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeskBooker.DataAccess.Tests
{
    public class DeskBookingRepositoryTests
    {
        [Test]
        public void ShouldSaveTheDeskBooking()
        {
            //arrange
            var options = new DbContextOptionsBuilder<DeskBookerContext>()
              .UseInMemoryDatabase(databaseName: "ShouldSaveTheDeskBooking")
              .Options;

            var deskBooking = new DeskBooking
            {
                FirstName = "John",
                LastName = "Doe",
                Date = new DateTime(2021, 5, 5),
                Email = "john.doe@email.com",
                DeskId = 1
            };

            //act
            using (var context = new DeskBookerContext(options))
            {
                var repository = new DeskBookingRepository(context);
                repository.Save(deskBooking);
            }

            //assert
            using (var context = new DeskBookerContext(options))
            {
                var bookings = context.DeskBookings.ToList();
                var storedDeskBooking = bookings.First();

                Assert.That(bookings, Has.Count.EqualTo(1));

                Assert.That(deskBooking.FirstName, Is.EqualTo(storedDeskBooking.FirstName));
                Assert.That(deskBooking.LastName, Is.EqualTo(storedDeskBooking.LastName));
                Assert.That(deskBooking.Email, Is.EqualTo(storedDeskBooking.Email));
                Assert.That(deskBooking.DeskId, Is.EqualTo(storedDeskBooking.DeskId));
                Assert.That(deskBooking.Date, Is.EqualTo(storedDeskBooking.Date));
            }
        }

        [Test]
        public void ShouldGetAllOrderedByDate()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DeskBookerContext>()
              .UseInMemoryDatabase(databaseName: "ShouldGetAllOrderedByDate")
              .Options;

            var storedList = new List<DeskBooking>
            {
              CreateDeskBooking(1,new DateTime(2020, 1, 27)),
              CreateDeskBooking(2,new DateTime(2020, 1, 25)),
              CreateDeskBooking(3,new DateTime(2020, 1, 29))
            };

            var expectedList = 
                storedList
                .OrderBy(x => x.Date)
                .ToList();

            using (var context = new DeskBookerContext(options))
            {
                foreach (var deskBooking in storedList)
                {
                    context.Add(deskBooking);
                }

                context.SaveChanges();
            }

            // Act
            List<DeskBooking> actualList;
            using (var context = new DeskBookerContext(options))
            {
                var repository = new DeskBookingRepository(context);
                actualList = repository.GetAll().ToList();
            }

            // Assert
            CollectionAssert.AreEqual(expectedList, actualList, new DeskBookingEqualityComparer());
        }

        private class DeskBookingEqualityComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var deskBooking1 = (DeskBooking)x;
                var deskBooking2 = (DeskBooking)y;
                if (deskBooking1.Id > deskBooking2.Id)
                    return 1;
                if (deskBooking1.Id < deskBooking2.Id)
                    return -1;
                else
                    return 0;
            }
        }

        private DeskBooking CreateDeskBooking(int id, DateTime dateTime)
        {
            return new DeskBooking
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Date = new DateTime(2021, 5, 5),
                Email = "john.doe@email.com",
                DeskId = 1
            };
        }
    }
}
