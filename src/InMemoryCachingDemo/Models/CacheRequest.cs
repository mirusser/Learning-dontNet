using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryCachingDemo.Models
{
    public class CacheRequest
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
