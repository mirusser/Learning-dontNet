using Core.Enums;
using Core.Interfaces;
using Hangfire;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CacheTech _cacheTech;
        private readonly string _cacheKey;
        private readonly ApplicationDbContext _dbContext;
        private readonly Func<CacheTech, ICacheService> _cacheService;

        public GenericRepository(
            ApplicationDbContext dbContext, 
            Func<CacheTech, ICacheService> cacheService)
        {
            _cacheTech = CacheTech.Memory;
            _cacheKey = $"{typeof(T)}";

            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (!_cacheService(_cacheTech).TryGet(_cacheKey, out IReadOnlyList<T> cachedList))
            {
                cachedList = await _dbContext.Set<T>().ToListAsync();
                _cacheService(_cacheTech).Set(_cacheKey, cachedList);
            }

            return cachedList;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            BackgroundJob.Enqueue(() => RefreshCache());

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task RefreshCache()
        {
            _cacheService(_cacheTech).Remove(_cacheKey);
            var cachedList = await _dbContext.Set<T>().ToListAsync();

            _cacheService(_cacheTech).Set(_cacheKey, cachedList);
        }
    }
}
