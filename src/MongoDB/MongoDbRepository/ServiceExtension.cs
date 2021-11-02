using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDbRepository.Factory;
using MongoDbRepository.Repository;
using MongoDbRepository.Settings;

namespace MongoDbRepository
{
    public static class ServiceExtensions
    {
        public static void AddMongoRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));

            services.AddTransient(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddSingleton(typeof(IMongoCollectionFactory<>), typeof(MongoCollectionFactory<>));
        }
    }
}