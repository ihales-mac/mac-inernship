using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    public class RijndaelClass
    {

        public static byte[] PadBytesArray(ref byte[] bytes) {
            return bytes;

        }
        public static byte[] TruncateBytesArray(ref byte[] bytes) {

            int i = bytes.Length - 1;
            while (bytes[i] == 0)
            {
                i--;
            }

            Array.Resize(ref bytes, i + 1);
            return bytes;
        }
       
        public static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] IV)
        {
         
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = key;
                rijAlg.IV = IV;
                rijAlg.Padding = PaddingMode.None;
                Console.WriteLine("Encryption key {0}",Convert.ToBase64String(rijAlg.Key));
                Console.WriteLine("Encryption vector {0}",Convert.ToBase64String(rijAlg.IV));
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                       // csEncrypt.FlushFinalBlock();
                    }
                    
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] IV)
        {
     
            //check arguments
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Rijndael object
            // with the specified key and IV.
            using (Rijndael rijAlg = Rijndael.Create())
            {
                rijAlg.Key = key;
                rijAlg.IV = IV;
                rijAlg.Padding = PaddingMode.None;
                Console.WriteLine("Decryption key {0}", Convert.ToBase64String(rijAlg.Key));
                Console.WriteLine("Decryption vector {0}", Convert.ToBase64String(rijAlg.IV));

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();

                        }
                       // csDecrypt.FlushFinalBlock();
                    }
                }

            }

            return plaintext;

        }

    }

}
