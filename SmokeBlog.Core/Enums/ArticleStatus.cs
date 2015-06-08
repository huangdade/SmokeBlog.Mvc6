using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Enums
{
    public enum ArticleStatus : byte
    {
        Delete = 0,
        Save = 1,
        Publish = 2
    }
}
