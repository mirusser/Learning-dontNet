using DeskBooker.Core.Domain;
using DeskBooker.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeskBooker.DataAccess.Tests.Repositories
{
    public class DeskRepositoryTests
    {
        [Test]
        public void ShouldReturnTheAvailableDesks()
        {
            var date = new DateTime(2021, 5, 5);

            var options =
                new DbContextOptionsBuilder<DeskBookerContext>()
                .UseInMemoryDatabase(databaseName: "ShouldReturnTheAvailableDesks")
                .Options;

            using (var context = new DeskBookerContext(options))
            {
                context.Desk.Add(new Desk { Id = 1 });
                context.Desk.Add(new Desk { Id = 2 });
                context.Desk.Add(new Desk { Id = 3 });

                context.DeskBookings.Add(new DeskBooking() { DeskId = 1, Date = date });
                context.DeskBookings.Add(new DeskBooking() { DeskId = 2, Date = date.AddDays(1) });

                context.SaveChanges();
            }

            using (var context = new DeskBookerContext(options))
            {
                var repository = new DeskRepository(context);

                var desks = repository.GetAvailableDesks(date);

                Assert.That(desks, Has.Count.EqualTo(2));
            }
        }

        [Test]
        public void ShouldGetAll()
        {
            //arrange
            var options =
                new DbContextOptionsBuilder<DeskBookerContext>()
                .UseInMemoryDatabase(databaseName: "ShouldGetAll")
                .Options;

            var storedList = new List<Desk>
            {
                new Desk(),
                new Desk(),
                new Desk()
            };

            using (var context = new DeskBookerContext(options))
            {
                foreach (var desk in storedList)
                {
                    context.Add(desk);
                }
                context.SaveChanges();
            }

            //act
            List<Desk> actualList;
            using (var context = new DeskBookerContext(options))
            {
                var repository = new DeskRepository(context);
                actualList = repository.GetAll().ToList();
            }

            //assert
            Assert.That(storedList.Count, Is.EqualTo(actualList.Count));

        }
    }
}
