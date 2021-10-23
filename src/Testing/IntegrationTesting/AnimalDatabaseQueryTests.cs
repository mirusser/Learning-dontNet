using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTesting
{
    [Collection(nameof(AnimalCollection))]
    public class AnimalDatabaseQueryTests
    {
        private readonly AnimalSetupFixture _animalSetupFixture;

        public AnimalDatabaseQueryTests(AnimalSetupFixture animalSetupFixture)
        {
            _animalSetupFixture = animalSetupFixture;
        }

        [Fact]
        public async Task Should_AnimalStore_ListsAnimalFromDatabase()
        {
            //arrange
            //AnimalSetupFixture is seeding the databse

            //act
            var animals = await _animalSetupFixture.Store.GetAnimals();

            //assert
            Assert.True(animals.Count >= 3);
            Assert.Contains(animals, x => x.Name.Equals("Foo"));
            Assert.Contains(animals, x => x.Name.Equals("Bar"));
            Assert.Contains(animals, x => x.Name.Equals("Baz"));
        }
    }
}
