using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Extensions
{
    public static class StringExtensions
    {
        public static int[] ToIntArray(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new int[0];
            }

            var arr = input.Split(',');

            return arr.Select(t => Convert.ToInt32(t)).ToArray();
        }
    }
}
