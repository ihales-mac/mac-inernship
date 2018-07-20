using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    public class HashesClass
    {


        public static String ComputeHash(String message, String algo)
        {
            byte[] sourceBytes = Encoding.Default.GetBytes(message);
            byte[] hashBytes = null;

            switch (algo.Trim().ToUpper())
            {
                case "MD5":
                    hashBytes = MD5CryptoServiceProvider.Create().ComputeHash(sourceBytes);
                    break;
                case "SHA1":
                    hashBytes = SHA1Managed.Create().ComputeHash(sourceBytes);
                    break;
                case "SHA256":
                    hashBytes = SHA256Managed.Create().ComputeHash(sourceBytes);
                    break;
                case "SHA384":
                    hashBytes = SHA384Managed.Create().ComputeHash(sourceBytes);
                    break;
                case "SHA512":
                    hashBytes = SHA512Managed.Create().ComputeHash(sourceBytes);
                    break;
                default:
                    break;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; hashBytes != null & i < hashBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", hashBytes[i]);
            }
            return sb.ToString();
        }


    }
}
