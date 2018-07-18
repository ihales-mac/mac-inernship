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
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            PublicKey = rsa.ToXmlString(false);
            _privateKey = rsa.ToXmlString(true);
        }

        public string EncryptText(string publicKey, string text)
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = byteConverter.GetBytes(text);
            byte[] encryptedData;
            using (RSACryptoServiceProvider tempRsa = new RSACryptoServiceProvider())
            {
                tempRsa.FromXmlString(publicKey);
                encryptedData = tempRsa.Encrypt(dataToEncrypt, false);
            }
            return Encoding.ASCII.GetString(encryptedData, 0, encryptedData.Length);
        }

        public string DecryptData(string text)
        {
            byte[] dataToDecrypt = Encoding.ASCII.GetBytes(text);
            byte[] decryptedData;
            using (RSACryptoServiceProvider tempRsa = new RSACryptoServiceProvider())
            {
                tempRsa.FromXmlString(_privateKey);
                decryptedData = tempRsa.Decrypt(dataToDecrypt, false);
            }
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            return byteConverter.GetString(decryptedData);
        }
    }
}
