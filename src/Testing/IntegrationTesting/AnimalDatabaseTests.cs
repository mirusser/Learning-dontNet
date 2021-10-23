using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTesting.Components.Database;
using Npgsql;
using Xunit;

namespace IntegrationTesting
{
    public class AnimalDatabaseTests
    {
        public const string _connBase = "Server=127.0.0.1;Port=5432;Database=postgres;User Id=username;Password=password;";
        public const string _db = "test_db";
        public static readonly string _conn = _connBase.Replace("postgres", _db);

        [Fact]
        public async Task Should_AnimalStore_SavesAnimalToDatabase()
        {
            await DatabaseSetup.CreateDatabase(_connBase, _db);

            var connectionFactory = new PostgresqlConnectionFactory(_conn);
            NpgsqlConnection connection = await connectionFactory.Create();
            IDatabase database = new Postgresql(connection);
            IAnimalStore store = new AnimalStore(database);

            await store.SaveAnimal(new (0, "Foo", "Bar"));

            var animals = await store.GetAnimals();

            await connectionFactory.DisposeAsync();
            await DatabaseSetup.DeleteDatabase(_connBase, _db);

            var animal = Assert.Single(animals);
            Assert.Equal(1, animal.Id);
            Assert.Equal("Foo", animal.Name);
            Assert.Equal("Bar", animal.Type);

        }
    }
}
