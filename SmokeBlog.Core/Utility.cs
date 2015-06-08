using Microsoft.AspNet.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static string HMACSHA1Encrypt(string input, string salt)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] result = KeyDerivation.Pbkdf2(input, saltBytes, KeyDerivationPrf.HMACSHA1, 10000, 16);

            return BitConverter.ToString(result).Replace("-", "");
        }

        public static string MD5(string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();

            byte[] data = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] hash = md5.ComputeHash(data);
            String result = BitConverter.ToString(hash).Replace("-", "");
            return result;
        }
    }
}
