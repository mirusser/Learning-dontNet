using System.Threading.Tasks;
using IntegrationTesting.Components.Database;
using Npgsql;
using Xunit;

namespace IntegrationTesting
{
    public class AnimalSetupFixture : IAsyncLifetime
    {
        private const string _connBase = "Server=127.0.0.1;Port=5432;Database=postgres;User Id=username;Password=password;";
        private const string _db = "animal_setup_fixture";
        private static readonly string _conn = _connBase.Replace("postgres", _db);
        private PostgresqlConnectionFactory _connectionFactory;

        public AnimalStore Store { get; private set; }

        public async Task InitializeAsync()
        {
            await DatabaseSetup.CreateDatabase(_connBase, _db);

            _connectionFactory = new(_conn);
            NpgsqlConnection connection = await _connectionFactory.Create();
            var database = new Postgresql(connection);

            Store = new AnimalStore(database);
            await Seed();
        }

        public async Task Seed()
        {
            await Store.SaveAnimal(new(0, "Foo", "Bar"));
            await Store.SaveAnimal(new(0, "Bar", "Bar"));
            await Store.SaveAnimal(new(0, "Baz", "Bar"));
        }

        public async Task DisposeAsync()
        {
            await _connectionFactory.DisposeAsync();
            await DatabaseSetup.DeleteDatabase(_connBase, _db);
        }
    }
}