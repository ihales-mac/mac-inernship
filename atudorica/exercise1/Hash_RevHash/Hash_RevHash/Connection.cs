using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Hash_RevHash
{
    class Connection
    {
        private string connectionString;
        public Connection()
        {
            connectionString = "Data Source=(local);Initial Catalog=exercise2;" + "Integrated Security=true";
        }

        public void insertBulk(List<string> passwords, int startIndex, int endIndex)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    DataTable dt = new DataTable("passwords");
                    dt.Columns.Add("Password");
                    dt.Columns.Add("Md5Hash");
                    dt.Columns.Add("Sha2Hash");
                    dt.Columns.Add("Sha1Hash");

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Password"] = passwords[i];
                        dr["Md5Hash"] = HashMethods.GetMd5Hash(passwords[i]);
                        dr["Sha1Hash"] = HashMethods.GetSHA1Hash(passwords[i]);
                        dr["Sha2Hash"] = HashMethods.GetSHA2Hash(passwords[i]);
                        dt.Rows.Add(dr);
                    }
                    SqlBulkCopy sbc = new SqlBulkCopy(cn);
                    SqlBulkCopyColumnMapping mapPass = new SqlBulkCopyColumnMapping("Password", "Password");
                    sbc.ColumnMappings.Add(mapPass);
                    SqlBulkCopyColumnMapping mapMd5Hash = new SqlBulkCopyColumnMapping("Md5Hash", "Md5Hash");
                    sbc.ColumnMappings.Add(mapMd5Hash);
                    SqlBulkCopyColumnMapping mapSha1Hash = new SqlBulkCopyColumnMapping("Sha1Hash", "Sha1Hash");
                    sbc.ColumnMappings.Add(mapSha1Hash);
                    SqlBulkCopyColumnMapping mapSha2Hash = new SqlBulkCopyColumnMapping("Sha2Hash", "Sha2Hash");
                    sbc.ColumnMappings.Add(mapSha2Hash);
                    sbc.DestinationTableName = "dbo.Hash";
                    sbc.BatchSize = 10000;
                    sbc.WriteToServer(dt);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void insertLines(List<string> passwords, int startIndex, int endIndex)
        {

            string query = "INSERT INTO dbo.Hash (Password,Md5Hash,Sha1Hash,Sha2Hash) "
                                         + "VALUES (@password, @md5hash, @sha1hash, @sha2hash) ";
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.CommandTimeout = 900;
                cn.Open();
                SqlTransaction transaction;
                transaction = cn.BeginTransaction("transaction");
                cmd.Transaction = transaction;
                try
                {
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = passwords[i];
                        cmd.Parameters.Add("@md5hash", SqlDbType.NVarChar, 32).Value = HashMethods.GetMd5Hash(passwords[i]);
                        cmd.Parameters.Add("@sha1hash", SqlDbType.NVarChar, 40).Value = HashMethods.GetSHA1Hash(passwords[i]);
                        cmd.Parameters.Add("@sha2hash", SqlDbType.NVarChar, 64).Value = HashMethods.GetSHA2Hash(passwords[i]);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    transaction.Commit();
                    Console.WriteLine("Multiple rows are written.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
        }

        public void InsertData(string password)
        {
            string query = "INSERT INTO dbo.Hash (Password,Md5Hash,Sha1Hash,Sha2Hash) "
                                        + "VALUES (@password, @md5hash, @sha1hash, @sha2hash) ";
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.CommandTimeout = 900;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password;
                cmd.Parameters.Add("@md5hash", SqlDbType.NVarChar, 32).Value = HashMethods.GetMd5Hash(password);
                cmd.Parameters.Add("@sha1hash", SqlDbType.NVarChar, 40).Value = HashMethods.GetSHA1Hash(password);
                cmd.Parameters.Add("@sha2hash", SqlDbType.NVarChar, 64).Value = HashMethods.GetSHA2Hash(password);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public void CleanDatabase()
        {
            string query = "Delete from dbo.Hash; DBCC CHECKIDENT ('[Hash]', RESEED, 0); ";
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cn.Open();
                cmd.CommandTimeout = 900;
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public string GetPasswordFromHash(string hash)
        {
            string result=null;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[GetPasswordFromHash]", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@HashValue", SqlDbType.NVarChar).Value = hash;
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    result =reader[0].ToString();
                }
            }
            return result;
        }
    }
}
