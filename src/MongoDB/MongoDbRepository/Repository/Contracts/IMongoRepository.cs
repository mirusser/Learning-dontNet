using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDbRepository.Repository
{
    /// <summary>
    /// Wrapper around IMongoCollection
    /// </summary>
    /// <typeparam name="TMongoDocument"></typeparam>
    public interface IMongoRepository<TMongoDocument> where TMongoDocument : class
    {
        #region Read

        Task<bool> ExistsAsync(Expression<Func<TMongoDocument, bool>> predicate, CancellationToken cancellation = default);

        Task<bool> ExistsAsync(Expression<Func<TMongoDocument, bool>> predicate, CountOptions? findOptions = null, CancellationToken cancellation = default);

        Task<TMongoDocument?> FindOneAsync(Expression<Func<TMongoDocument, bool>> predicate, CancellationToken cancellation = default);

        Task<TMongoDocument?> FindOneAsync(Expression<Func<TMongoDocument, bool>> predicate, FindOptions? findOptions = null, CancellationToken cancellation = default);

        Task<IEnumerable<TMongoDocument>> GetAllAsync(CancellationToken cancellation = default);

        Task<IEnumerable<TMongoDocument>> GetAllAsync(FindOptions<TMongoDocument, TMongoDocument>? findOptions = null, CancellationToken cancellation = default);

        Task<IEnumerable<TMongoDocument>> FindManyAsync(Expression<Func<TMongoDocument, bool>> predicate, FindOptions<TMongoDocument, TMongoDocument>? findOptions = null, CancellationToken cancellation = default);

        //TODO: other read methods here ...

        #endregion Read

        #region Write

        Task CreateOneAsync(TMongoDocument mongoDocument, InsertOneOptions? insertOneOptions = null, CancellationToken cancellation = default);

        Task CreateManyAsync(IEnumerable<TMongoDocument> mongoDocuments, InsertManyOptions? insertManyOptions = null, CancellationToken cancellation = default);

        Task<bool> ReplaceOneAsync(TMongoDocument mongoDocument, Expression<Func<TMongoDocument, bool>> predicate, ReplaceOptions? replaceOptions = null, CancellationToken cancellation = default);

        Task<bool> ReplaceManyAsync(UpdateDefinition<TMongoDocument> updateDefinition, Expression<Func<TMongoDocument, bool>> predicate, UpdateOptions? updateOptions = null, CancellationToken cancellation = default);

        Task<bool> DeleteOneAsync(Expression<Func<TMongoDocument, bool>> predicate, CancellationToken cancellation = default);

        Task<bool> DeleteManyAsync(Expression<Func<TMongoDocument, bool>> predicate, CancellationToken cancellation = default);

        #endregion Write
    }
}