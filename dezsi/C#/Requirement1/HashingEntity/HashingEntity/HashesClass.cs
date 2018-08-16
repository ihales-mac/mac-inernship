using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashingEntity
{
    public class HashesClass
    {

        public static String ReverseHash(String hash, String type)
        {

            string connectionString =
           "Data Source=DESKTOP-4KOH9RJ;" +
           "Initial Catalog=Hashing;"
           + "Integrated Security=true";
            string queryString = "SELECT password FROM Rainbow where ";
            switch (type)
            {

                case "MD5":
                    queryString += "md5hash = '" + hash + "'";
                    break;
                case "SHA1":
                    queryString += "sha1hash = '" + hash + "'";
                    break;
                case "SHA256":
                    queryString += "sha2hash = '" + hash + "'";
                    break;
                default:
                    break;
            };


            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(queryString, connection);
            String password = (String)command.ExecuteScalar();

            connection.Close();
            return password;

        }

        public static String computeHash(String message, String algo)
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
