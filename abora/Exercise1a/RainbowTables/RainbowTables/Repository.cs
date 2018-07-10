using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;



namespace RainbowTables
{
    class Repository
    {
        List<string> allCombinations = new List<string>();

        public Repository()
        {

            List<string> permutations = Utils.getPermutations();
            List<string> permutationsMultiple10 = new List<string>();

            this.clearRainbowTable();
            


            int i = 0;

            //Populate Database

            //v1
            //insert a row at a time

            /*
            var watch1 = System.Diagnostics.Stopwatch.StartNew();
            foreach (string password in permutations)
            {

                this.insertRowAtATimePassword(password);
            }
            watch1.Stop();
            var elapsedMs = watch1.ElapsedMilliseconds;
            Console.WriteLine("Populate DB one at a time : {0} ms",elapsedMs);
            */

            //v2
            //insert multiple rows at a time
            //By using batches
           
           /*
            var watch2 = System.Diagnostics.Stopwatch.StartNew();
            foreach (string password in permutations)
            {
                
                
                i++;
                if(i % 10 == 0)
                {
                    //insert multiple rows
                    this.insertMultipleRowAtATimePassword(permutationsMultiple10);
                    permutationsMultiple10.Clear();
                }
                permutationsMultiple10.Add(password);
                
            }
            this.insertMultipleRowAtATimePassword(permutationsMultiple10);
            watch2.Stop();
            var elapsedMs2 = watch2.ElapsedMilliseconds;
            Console.WriteLine("Populate DB multiple at a time : {0} ms", elapsedMs2);
            */
            
        }


        /*
         * Insert multiple row at a time using 
         * a stored procedure
         */
        private void insertMultipleRowAtATimePassword(List<string> permutationsMultiple10)
        {
            DataTable dataTable = new DataTable("RainbowTableType");
            dataTable.Columns.Add("password", typeof(string));
            foreach(string permutation in permutationsMultiple10)
            {
                dataTable.Rows.Add(permutation);
            }

            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = "@tvpNewRainbowTable";
            parameter.SqlDbType = System.Data.SqlDbType.Structured;
            parameter.Value = dataTable;

            using (SqlConnection conn = new SqlConnection(getConnString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.usp_insertPass", conn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(parameter);
                    conn.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }

        }
        /*
         *Make a backup for the Rainbow table 
         */
        public void copyPassToAnotherTable()
        {
            string query = "SELECT * FROM Rainbow";

            using (SqlConnection conn = new SqlConnection(getConnString() + "MultipleActiveResultSets=true;"))
            {
                using (SqlCommand sqlCommand = new SqlCommand(query, conn))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    SqlBulkCopy sqlBulk = new SqlBulkCopy(conn);
                    sqlBulk.DestinationTableName = "RainbowCpy";
                    sqlBulk.WriteToServer(reader);
                }
            }



        }
        /*
         * Remove all data from Rainbow table 
         */
        public void clearRainbowTable()
        {
            using (SqlConnection conn = new SqlConnection(getConnString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.clearRainbow", conn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        /*
         *Get the password of a user in a string format 
         */
        public string getPassword(string username)
        {
            var watch3 = System.Diagnostics.Stopwatch.StartNew();

            string password = null;
            using (SqlConnection conn = new SqlConnection(getConnString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select dbo.getPassFromHashfunction(@username)", conn))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@username", username);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            password = reader[0].ToString();
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }   
                }
            }

            watch3.Stop();
            var elapsedMs3 = watch3.ElapsedMilliseconds;
            Console.WriteLine("search time: {0} ms", elapsedMs3);

            return password;
        }
        
        /*
         * Add a user in DB for debugging purpose
         */
        public void add(string username, string password, string hashAlgorithm)
        {

            using (SqlConnection conn = new SqlConnection(getConnString()))
            {
                using(SqlCommand sqlCommand = new SqlCommand("dbo.insertUser", conn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@username", username);
                    sqlCommand.Parameters.AddWithValue("@password", password);
                    sqlCommand.Parameters.AddWithValue("@hashAlgorithm", hashAlgorithm);
                    conn.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            
        }
        /*
         * Insert a Row at a time
         * using a stored procedure
         */
        public void insertRowAtATimePassword(string password)
        {
            //V1 withow transactions
            /*
            using (SqlConnection conn = new SqlConnection(getConnString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand("dbo.insertPass", conn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@password", password);
                    conn.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            */

            //v2 with Transactions
            //Each insert in a separate transaction
            using (SqlConnection conn = new SqlConnection(getConnString()))
            {
                conn.Open();
                SqlTransaction sqlTransaction = conn.BeginTransaction();
                SqlCommand command = conn.CreateCommand();
                command.Transaction = sqlTransaction;

               
                try
                {
                    command.CommandText = "dbo.insertPass";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch(Exception ex1)
                {
                    try
                    {
                        sqlTransaction.Rollback();
                    }catch(Exception ex2)
                    {
                        Console.WriteLine(ex2.Message);
                    }
                }
            }

        }


        private string getConnString()
        {
            return "Data Source =DESKTOP-FT60R8D\\SQLEXPRESS;" +
                "Initial Catalog = Ex1v2;" +
                "Integrated Security = true;";
        }
    }
}
