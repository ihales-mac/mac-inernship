using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RainbowEF
{
    public static class Hashing
    {
        public static string EncodeMD5(string password)
        {
            byte[] encodedBytes;

            using (var md5 = MD5.Create())
            {
                var originalBytes = Encoding.UTF8.GetBytes(password);
                encodedBytes = md5.ComputeHash(originalBytes);
            }
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < encodedBytes.Length; i++)
            {
                sBuilder.Append(encodedBytes[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string EncodeSHA1(string password)
        {
            byte[] encodedBytes;
            using(var sha1 = new SHA1CryptoServiceProvider())
            {
                var originalBytes = Encoding.UTF8.GetBytes(password);
                encodedBytes = sha1.ComputeHash(originalBytes);
            }
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < encodedBytes.Length; i++)
            {
                sBuilder.Append(encodedBytes[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public static string EncodeSHA256(string password)
        {
            byte[] encodedBytes;
            using (var sha256 = SHA256Managed.Create())
            {
                var originalBytes = Encoding.UTF8.GetBytes(password);
                encodedBytes = sha256.ComputeHash(originalBytes);
            }

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < encodedBytes.Length; i++)
            {
                sBuilder.Append(encodedBytes[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /*
         * REASEARCH
         * Adding salt to one of the above algorithms 
         * makes the dictionary or rainbow tables attack 
         * much harder because besides trying the most commonly
         * used password they have to guess the salt as well.
         * 
         * A better approach is to use prepend the password with
         * the salt and use one of the following algoritms
         * BCrypt, SCrypt, Argon2 and PBKDF2.
         */

        public static string EncodeSHA256Salt(string password)
        {
            string salt = CreateSalt(32);
            //salt must be saved in DB
            //and be different for each user
            byte[] encodedBytes;
            using (var sha256 = SHA256Managed.Create())
            {
                var originalBytes = Encoding.UTF8.GetBytes(password+salt);
                encodedBytes = sha256.ComputeHash(originalBytes);
            }
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < encodedBytes.Length; i++)
            {
                sBuilder.Append(encodedBytes[i].ToString("x2"));
            }
            return sBuilder.ToString();

        }

        private static string CreateSalt(int size)
        {
            var rng = RNGCryptoServiceProvider.Create();
            var buffer = new byte[size];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }
}
