using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbRepository.Settings;

namespace MongoDbRepository.Factory
{
    public class MongoCollectionFactory<TMongoDocument> : IMongoCollectionFactory<TMongoDocument> where TMongoDocument : class
    {
        private readonly MongoSettings _settings;

        public MongoCollectionFactory(IOptions<MongoSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Gets or create mongo collection, depending on whether it already exists in the database
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public IMongoCollection<TMongoDocument> Get(string? collectionName = null)
        {
            collectionName = !string.IsNullOrEmpty(collectionName) ? collectionName : typeof(TMongoDocument).Name;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.Database);

            if (!database.ListCollectionNames().ToList().Any(c => c == collectionName))
            {
                database.CreateCollection(collectionName);
            }

            return database.GetCollection<TMongoDocument>(collectionName);
        }
    }
}
