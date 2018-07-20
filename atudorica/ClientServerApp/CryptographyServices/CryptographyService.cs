using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyServices
{
    public class CryptographyService
    {
        public string PublicKey;
        private readonly string _privateKey;
        public CryptographyService()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            PublicKey = rsa.ToXmlString(false);
            _privateKey = rsa.ToXmlString(true);
        }

        public byte[] EncryptText(string publicKey, string text)
        {
            byte[] dataToEncrypt = Convert.FromBase64String(text);
            byte[] encryptedData;
            using (RSACryptoServiceProvider tempRsa = new RSACryptoServiceProvider())
            {
                tempRsa.FromXmlString(publicKey);
                encryptedData = tempRsa.Encrypt(dataToEncrypt, false);
            }
            return encryptedData;
        }

        public string DecryptData(byte[] dataToDecrypt)
        {
            byte[] decryptedData;
            using (RSACryptoServiceProvider tempRsa = new RSACryptoServiceProvider())
            {
                tempRsa.FromXmlString(_privateKey);
                decryptedData = tempRsa.Decrypt(dataToDecrypt, false);
            }
            return Convert.ToBase64String(decryptedData);
        }
    }
}
