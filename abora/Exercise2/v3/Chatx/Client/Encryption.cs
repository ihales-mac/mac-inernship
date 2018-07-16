using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Client
{
    class Encryption
    {
        public Encryption()
        {

        }

        static string EncryptText(string publicKey, byte[] dataToEncrypt)
        {
            // Create a byte array to store the encrypted data in it   
            byte[] encryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Set the rsa public key   
                rsa.FromXmlString(publicKey);

                // Encrypt the data and store it in the encyptedData Array   
                encryptedData = rsa.Encrypt(dataToEncrypt, true);
            }

            return Encoding.Unicode.GetString(encryptedData);
        }
    }
}
