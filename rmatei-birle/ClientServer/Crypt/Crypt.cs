using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypt
{
    public static class Crypt
    {
        private static readonly RijndaelManaged _rijndaelRMCrypto = new RijndaelManaged();

        private static byte[] _key = { 0x43, 0x32, 0x33, 0x56, 0x21, 0x56, 0x86, 0x67, 0x28, 0x45, 0x24, 0x38, 0x47, 0x59, 0x16, 0x23 };
        private static byte[] _iv = { 0x23, 0x84, 0x65, 0x94, 0x74, 0x48, 0x65, 0x79, 0x45, 0x86, 0x98, 0x54, 0x23, 0x68, 0x83, 0x96 };

        private static readonly ICryptoTransform _encryptor = _rijndaelRMCrypto.CreateEncryptor(_key, _iv);
        private static readonly ICryptoTransform _decryptor = _rijndaelRMCrypto.CreateDecryptor(_key, _iv);
        
        public static string Encrypt(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            byte[] encrypted = _encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Encoding.ASCII.GetString(encrypted);
        }

        public static string Decrypt(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            byte[] decrypted = _decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Encoding.ASCII.GetString(decrypted);
        }


    }
}
