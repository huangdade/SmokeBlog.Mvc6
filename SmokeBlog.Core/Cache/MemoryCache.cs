using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Cache
{
    public sealed class MemoryCache : ICache
    {
        private static readonly System.Runtime.Caching.MemoryCache cache = System.Runtime.Caching.MemoryCache.Default;

        public void Delete(string key)
        {
            cache.Remove(key);
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }

        public T Get<T>(string key)
        {
            var obj = this.Get(key);

            if (obj == null)
            {
                return default(T);
            }

            return (T)obj;
        }

        public void Set(string key, object value)
        {
            cache.Set(key, value, DateTime.Now.AddMinutes(20));
        }
    }
}
