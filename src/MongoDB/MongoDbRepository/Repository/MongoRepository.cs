using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDbRepository.Factory;

namespace MongoDbRepository.Repository
{
    /// <summary>
    /// Wrapper around IMongoCollection
    /// </summary>
    /// <typeparam name="TMongoDocument"></typeparam>
    public class MongoRepository<TMongoDocument> : IMongoRepository<TMongoDocument> where TMongoDocument : class
    {
        private readonly IMongoCollection<TMongoDocument> _mongoCollection;

        public MongoRepository(IMongoCollectionFactory<TMongoDocument> mongoCollectionFactory)
        {
            _mongoCollection = mongoCollectionFactory.Get();
        }

        #region Read
        public async Task<bool> ExistsAsync(Expression<Func<TMongoDocument, bool>> predicate, CancellationToken cancellation = default)
            => await ExistsAsync(predicate, null, cancellation).ConfigureAwait(false);

        public async Task<bool> ExistsAsync(Expression<Func<TMongoDocument, bool>> predicate, CountOptions? findOptions = null, CancellationToken cancellation = default)
            => (await _mongoCollection.CountDocumentsAsync(predicate, findOptions, cancellation).ConfigureAwait(false)) > default(long);

        public async Task<TMongoDocument?> FindOneAsync(Expression<Func<TMongoDocument, bool>> predicate, CancellationToken cancellation = default)
            => await FindOneAsync(predicate, null, cancellation).ConfigureAwait(false);

        public async Task<TMongoDocument?> FindOneAsync(Expression<Func<TMongoDocument, bool>> predicate, FindOptions? findOptions = null, CancellationToken cancellation = default)
            => await _mongoCollection.Find(predicate, findOptions).FirstOrDefaultAsync(cancellation).ConfigureAwait(false);

        public async Task<IEnumerable<TMongoDocument>> GetAllAsync(CancellationToken cancellation = default)
            => await GetAllAsync(cancellation).ConfigureAwait(false);

        public async Task<IEnumerable<TMongoDocument>> GetAllAsync(FindOptions<TMongoDocument, TMongoDocument>? findOptions = null, CancellationToken cancellation = default)
            => await FindManyAsync(_ => true, findOptions, cancellation).ConfigureAwait(false);

        public async Task<IEnumerable<TMongoDocument>> FindManyAsync(Expression<Func<TMongoDocument, bool>> predicate, FindOptions<TMongoDocument, TMongoDocument>? findOptions = null, CancellationToken cancellation = default)
            => (await _mongoCollection.FindAsync(predicate, findOptions, cancellation).ConfigureAwait(false)).ToEnumerable(cancellation);


        //TODO: other read methods here ...

        #endregion Read

        #region Write

        public async Task CreateOneAsync(TMongoDocument mongoDocument, InsertOneOptions? insertOneOptions = null, CancellationToken cancellation = default)
            => await _mongoCollection.InsertOneAsync(mongoDocument, insertOneOptions, cancellation).ConfigureAwait(false);

        public async Task CreateManyAsync(IEnumerable<TMongoDocument> mongoDocuments, InsertManyOptions? insertManyOptions = null, CancellationToken cancellation = default)
            => await _mongoCollection.InsertManyAsync(mongoDocuments, insertManyOptions, cancellation).ConfigureAwait(false);

        public async Task<bool> ReplaceOneAsync(TMongoDocument mongoDocument, Expression<Func<TMongoDocument, bool>> predicate, ReplaceOptions? replaceOptions = null, CancellationToken cancellation = default)
        {
            var result = await _mongoCollection.ReplaceOneAsync(predicate, mongoDocument, replaceOptions, cancellation).ConfigureAwait(false);
            return result.IsAcknowledged && result.IsModifiedCountAvailable && result.ModifiedCount > 0;
        }

        public async Task<bool> ReplaceManyAsync(UpdateDefinition<TMongoDocument> updateDefinition, Expression<Func<TMongoDocument, bool>> predicate, UpdateOptions? updateOptions = null, CancellationToken cancellation = default)
        {
            var result = await _mongoCollection.UpdateManyAsync(predicate, updateDefinition, updateOptions, cancellation);
            return result.IsAcknowledged && result.IsModifiedCountAvailable && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteOneAsync(Expression<Func<TMongoDocument, bool>> predicate, CancellationToken cancellation = default)
        {
            var result = await _mongoCollection.DeleteOneAsync(predicate, cancellation).ConfigureAwait(false);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> DeleteManyAsync(Expression<Func<TMongoDocument, bool>> predicate, CancellationToken cancellation = default)
        {
            var result = await _mongoCollection.DeleteManyAsync(predicate, cancellation).ConfigureAwait(false);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        //TODO: other write methods here ...

        #endregion Write
    }
}
