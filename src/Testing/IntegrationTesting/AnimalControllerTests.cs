using System.Collections.Generic;
using System.Reflection;
using IntegrationTesting.Components.Introduction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTesting
{
    public class AnimalControllerTests
    {
        [Fact]
        public void Should_AnimalController_ListAnimalsFromDatabase()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            using AppDbContext ctx = new(optionsBuilder.Options);
            ctx.Add(new Animal() { Id = 1, Name = "Foo", Type = "Bar" });
            ctx.SaveChanges();
            var result = new AnimalController(ctx).List();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var animals = Assert.IsType<List<Animal>>(okResult.Value);
            var animal = Assert.Single(animals);

            Assert.NotNull(animal);
            Assert.Equal(1, animal.Id);
            Assert.Equal("Foo", animal.Name);
            Assert.Equal("Bar", animal.Type);
        }

        [Fact]
        public void Should_AnimalController_GetsAnimalFromDatabase()
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            using AppDbContext ctx = new(optionsBuilder.Options);
            ctx.Add(new Animal() { Id = 1, Name = "Foo", Type = "Bar" });
            ctx.SaveChanges();
            var result = new AnimalController(ctx).Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var animal = Assert.IsType<Animal>(okResult.Value);

            Assert.NotNull(animal);
            Assert.Equal(1, animal.Id);
            Assert.Equal("Foo", animal.Name);
            Assert.Equal("Bar", animal.Type);
        }
    }
}