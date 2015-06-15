using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Cache
{
    public static class CacheExtensions
    {
        public static void Set(this ICache cache, string key, object value)
        {
            cache.Set(key, value, 20 * 60);
        }

        public static T Get<T>(this ICache cache, string key, Func<T> resolver)
        {
            if (cache.IsSet(key))
            {
                return cache.Get<T>(key);
            }
            else
            {
                T obj = resolver();
                cache.Set(key, obj);

                return obj;
            }
        }
    }
}
