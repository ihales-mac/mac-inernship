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
            int diff = 16 - bytes.Length % 16;
            if (diff < 0)
            {
                diff += 16;
            }
            if ((bytes.Length % 16) != 16)
            {
                byte[] newBytes = new byte[bytes.Length + diff];
                bytes.CopyTo(newBytes, 0);
                for (int i = 0; i < diff; i++)
                {
                    newBytes[bytes.Length + i] = 0;
                }

                bytes = new byte[bytes.Length + diff];
                newBytes.CopyTo(bytes, 0);
            }
            byte[] encrypted = _encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string message)
        {
            byte[] bytes = Convert.FromBase64String(message);
            byte[] decrypted = _decryptor.TransformFinalBlock(bytes, 0, bytes.Length);

            int lastCharIndex = decrypted.Length;

            for (int i = decrypted.Length - 1; i >= 0; i--)
            {
                if (decrypted[i] != 0)
                {
                    lastCharIndex = i;
                    break;
                }
            }

            byte[] aux = new byte[lastCharIndex + 1];

            for (int i = 0; i < aux.Length; i++)
            {
                aux[i] = decrypted[i];
            }

            bytes = aux;


            return Encoding.ASCII.GetString(bytes);
        }


    }
}
