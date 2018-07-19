using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Cryption
    {
        public static string Encrypt(string plainText,string passPhrase,string saltValue,string hashAlgorithm,
                                     int passwordIterations,string initVector,int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes password = new PasswordDeriveBytes
       (
           passPhrase,
           saltValueBytes,
           hashAlgorithm,
           passwordIterations
       );

            byte[] keyBytes = password.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor
        (
            keyBytes,
            initVectorBytes
        );

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream
        (
            memoryStream,
            encryptor,
            CryptoStreamMode.Write
        );

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }

        public static string Decrypt(string cipherText, string passPhrase, string saltValue, string hashAlgorithm, int passwordIterations,
                                        string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes password = new PasswordDeriveBytes
        (
            passPhrase,
            saltValueBytes,
            hashAlgorithm,
            passwordIterations
        );

            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor
       (
           keyBytes,
           initVectorBytes
       );
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream
       (
           memoryStream,
           decryptor,
           CryptoStreamMode.Read
       );
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read
            (
                plainTextBytes,
                0,
                plainTextBytes.Length
            );
            memoryStream.Close();
            cryptoStream.Close();
            string plainText = Encoding.UTF8.GetString
        (
            plainTextBytes,
            0,
            decryptedByteCount
        );
            return plainText;
        }

        public static  string EncryptMessage(string message)
        {
            string encrypt = Common.Cryption.Encrypt(message, "TestPassphrase", "TestSaltValue", "SHA256", 2, "!1A3g2D4s9K556g7", 256);
            return encrypt;
        }

        public static string DecryptMessage(string message)
        {
            string decrypt = Common.Cryption.Decrypt(message, "TestPassphrase", "TestSaltValue", "SHA256", 2, "!1A3g2D4s9K556g7", 256);
            return decrypt;
        }
    }
}
