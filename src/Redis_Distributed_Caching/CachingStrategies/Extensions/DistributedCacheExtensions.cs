using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CachingStrategies.Strategies;
using Microsoft.Extensions.Caching.Distributed;

namespace CachingStrategies.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static IDistributedCache ToNamespaced(this IDistributedCache @this, string name) =>
            new NamespacedCache(@this, name);
    }
}
