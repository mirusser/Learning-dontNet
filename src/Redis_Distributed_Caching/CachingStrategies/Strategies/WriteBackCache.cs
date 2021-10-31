using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace CachingStrategies.Strategies
{
    public class WriteBackCache : IDistributedCache
    {
        private readonly IDistributedCache _main;
        private readonly IDistributedCache _secondary;

        private List<KeyValuePair<string, byte[]>> writeBackBuffer = new();
        private readonly Task _backgroundTask;

        public WriteBackCache(IDistributedCache main, IDistributedCache secondary)
        {
            _main = main;
            _secondary = secondary;
            _backgroundTask = Task.Run(WriteBack);
        }

        private async Task WriteBack()
        {
            while (true)
            {
                try
                {
                    if (writeBackBuffer.Count > 100)
                    {
                        // build batch update
                    }

                    await Task.Delay(1000 * 60);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public byte[] Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Refresh(string key)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            _secondary.Set(key, value);
            writeBackBuffer.Add(KeyValuePair.Create(key, value));
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
