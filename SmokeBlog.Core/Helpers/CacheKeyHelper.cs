using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Helpers
{
    public sealed class CacheKeyHelper
    {
        public static string GetArticleCommentsCacheKey(int articleID)
        {
            string key = string.Format("Cache_Article_Comments_{0}", articleID.ToString());
            return key;
        }

        public static string GetLatestCommentsCacheKey()
        {
            string key = "Cache_Comment_Latest";
            return key;
        }

        public static string GetCategoryListCacheKey()
        {
            string key = "Cache_CategoryList";
            return key;
        }

        public static string GetLatestArticlesCacheKey()
        {
            string key = "Cache_Article_Latest";
            return key;
        }
    }
}
