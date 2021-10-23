using System;
using System.Threading.Tasks;
using IntegrationTesting.Components.Database;
using Npgsql;
using Xunit;

namespace IntegrationTesting
{
    public class AnimalDatabaseTests : IClassFixture<AnimalSetupFixture>
    {
        private readonly AnimalSetupFixture _animalSetupFixture;

        public AnimalDatabaseTests(AnimalSetupFixture animalSetupFixture)
        {
            _animalSetupFixture = animalSetupFixture;
        }

        [Fact]
        public async Task Should_AnimalStore_SavesAnimalToDatabase()
        {
            //arrange
            var name = Guid.NewGuid().ToString();
            await _animalSetupFixture.Store.SaveAnimal(new(0, name, "Bar"));

            //act
            var animals = await _animalSetupFixture.Store.GetAnimals();

            //assert
            Assert.Single(animals, x => x.Name.Equals(name));
        }

        [Fact]
        public async Task Should_AnimalStore_GetsSavedAnimalByIdFromDatabase()
        {
            //arrange
            //AnimalSetupFixture is seeding the databse

            //act
            var animal = await _animalSetupFixture.Store.GetAnimal(1);

            //assert
            Assert.NotNull(animal);
            Assert.Equal(1, animal.Id);
            Assert.Equal("Foo", animal.Name);
            Assert.Equal("Bar", animal.Type);
        }
    }
}