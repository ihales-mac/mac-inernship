using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Common
{
    public class EncryptionRSA
    {
        RSAParameters privateKey;
        RSAParameters publicKey;
        RSACryptoServiceProvider rsa;


        public EncryptionRSA()
        {
            rsa = new RSACryptoServiceProvider(1024);
            privateKey = rsa.ExportParameters(true);
            publicKey = rsa.ExportParameters(false);

        }

        public string GetPrivateKey()
        {
            string privKeyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, privateKey);
                //get the string from the stream
                privKeyString = sw.ToString();
            }

            return privKeyString;
        }

        public void SetPrivateKey(string privateKey)
        {
            var privateKeyRSA = Deserialiaze(privateKey);
            rsa.ImportParameters(privateKeyRSA);
        }

        public string GetPublicKey()
        {
            string pubKeyString;
            {
                //we need some buffer
                var sw = new System.IO.StringWriter();
                //we need a serializer
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                //serialize the key into the stream
                xs.Serialize(sw, publicKey);
                //get the string from the stream
                pubKeyString = sw.ToString();
            }

            return pubKeyString;
        }

        public static RSAParameters Deserialiaze(string key)
        {
            //get a stream from the string
            var sr = new System.IO.StringReader(key);
            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //get the object back from the stream
            return (RSAParameters)xs.Deserialize(sr);

        }

        public byte[] Encrypt(byte[] data)
        {
            //ecript using public key;
            return rsa.Encrypt(data, false);
        }

        public void SetPublicKey(string publicKeyStr)
        {
            var publicKey = Deserialiaze(publicKeyStr);
            rsa.ImportParameters(publicKey);
        }

        public string Decrypt(byte[] data)
        {
    
                byte[] decripted = rsa.Decrypt(data, false);
                return Encoding.ASCII.GetString(decripted);
    
      
        }
    }
}
