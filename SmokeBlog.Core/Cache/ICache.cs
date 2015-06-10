using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Cache
{
    public interface ICache
    {
        void Set(string key, object value);

        object Get(string key);

        T Get<T>(string key);

        void Delete(string key);
    }
}
