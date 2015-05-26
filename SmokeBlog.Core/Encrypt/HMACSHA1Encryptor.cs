using Microsoft.AspNet.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Encrypt
{
    public class HMACSHA1Encryptor
    {
        public static string Encrypt(string input, string salt)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] result = KeyDerivation.Pbkdf2(input, saltBytes, KeyDerivationPrf.HMACSHA1, 10000, 16);

            return BitConverter.ToString(result).Replace("-", "");
        }
    }
}
