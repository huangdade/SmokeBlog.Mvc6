using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core
{
    public sealed class Utility
    {
        private static Random Rnd { get; } = new Random();

        public static string GetRandomString(int length)
        {
            char[] code = "abcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();

            char[] result = new char[length];

            for (var i = 0; i < length; i++)
            {
                var chr = code[Rnd.Next(code.Length)];
                result[i] = chr;
            }

            return new string(result);
        }
    }
}
