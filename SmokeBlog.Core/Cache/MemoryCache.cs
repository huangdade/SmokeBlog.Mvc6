using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace SmokeBlog.Core.Cache
{
    public sealed class MemoryCache : ICache
    {
        private static readonly System.Runtime.Caching.MemoryCache cache = System.Runtime.Caching.MemoryCache.Default;

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();

            foreach (var item in cache)
                if (regex.IsMatch(item.Key))
                    keysToRemove.Add(item.Key);

            foreach (string key in keysToRemove)
            {
                Remove(key);
            }
        }

        public T Get<T>(string key)
        {
            return (T)cache[key];
        }

        public void Set(string key, object value, int seconds)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(seconds)
            };

            var item = new CacheItem(key, value);

            cache.Set(item, policy);
        }

        public bool IsSet(string key)
        {
            return cache.Contains(key);
        }
    }
}
