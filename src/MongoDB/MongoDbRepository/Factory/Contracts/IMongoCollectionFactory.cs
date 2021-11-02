using MongoDB.Driver;

namespace MongoDbRepository.Factory
{
    public interface IMongoCollectionFactory<TMongoDocument> where TMongoDocument : class
    {
        public IMongoCollection<TMongoDocument> Get(string? collectionName = null);
    }
}