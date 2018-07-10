using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rainbow_Saptamana1_
{
    public partial class Form1 : Form


    {
        SqlConnection connection = new SqlConnection("Data source=DESKTOP-MS32J8R; Initial Catalog=Rainbow; Integrated Security=True;MultipleActiveResultSets=true");

        SqlDataAdapter adapter = new SqlDataAdapter();
        DataSet set = new DataSet();
        List<String> list2 = new List<String> { "ana" };
        List<String> list = new List<string>();

      

        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("MD5");
            comboBox1.Items.Add("SHA1");
            comboBox1.Items.Add("SHA2");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //FUNCTIILE DE HASH
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public string SHA1(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {

                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public string Sha256Hash(string rawData)
        {

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private static int check(int c)
        {
            if (c > 122 || c < 97)
                return 1;
            return 0;
        }

        private static void printSolution(int[] vector)
        {
            Console.WriteLine((char)vector[0] + " " + (char)vector[1] + " " + (char)vector[2] + " " + (char)vector[3] + " " + (char)vector[4]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            String result;
            int k = 0;
            int[] vector = new int[] { 0, 0, 0, 0, 0 };
            while (k >= 0)
            {
                if (check(vector[k]) == 1)
                    vector[k] = 97;
                else
                    vector[k]++;

                if (k != 4 && check(vector[k]) == 0)
                    k++;
                else
                    if (check(vector[k]) == 0)
                {
                    // if ((char)nvector[0] == (char)vector[1] && (char)vector[0] == (char)vector[2] && (char)vector[0] == (char)vector[3] && (char)vector[0] == (char)vector[4])
                    // printSolution(vector);
                    try
                    {
                        char[] list = new char[] { (char)vector[0], (char)vector[1], (char)vector[2], (char)vector[3], (char)vector[4] };
                        result = new string(list);

                        adapter.InsertCommand = new SqlCommand("Insert Into Pass VALUES(@b,@c,@d,@e)", connection);
                        adapter.InsertCommand.Parameters.Add("@b", SqlDbType.VarChar).Value = result;
                        adapter.InsertCommand.Parameters.Add("@c", SqlDbType.VarChar).Value = CreateMD5(result);
                        adapter.InsertCommand.Parameters.Add("@d", SqlDbType.VarChar).Value = SHA1(result);
                        adapter.InsertCommand.Parameters.Add("@e", SqlDbType.VarChar).Value = Sha256Hash(result);




                        connection.Open();
                        adapter.InsertCommand.ExecuteNonQuery();
                        connection.Close();
                        if (result == "zzzzz")
                            k = -1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        connection.Close();
                    }

                }

                else
                    k--;
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}",
                 ts.Minutes, ts.Seconds);

            textBox1.Text = elapsedTime;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Type_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            String type = comboBox1.Text;
            String hash = textBox2.Text;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            adapter.SelectCommand = new SqlCommand("Select Pass From Pass Where " + type + "=@hash", connection);
            adapter.SelectCommand.Parameters.Add("@type", SqlDbType.VarChar).Value = type;
            adapter.SelectCommand.Parameters.Add("@hash", SqlDbType.VarChar).Value = hash;
            set.Clear();
            adapter.Fill(set);
            dataGridView1.DataSource = set.Tables[0];
            stopWatch.Stop();
            
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Minutes, ts.Seconds,
           ts.Milliseconds / 10);
            textBox1.Text = elapsedTime;





        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mydocpath =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath, "WriteLines2.txt")))
            {
                String result;
                int k = 0;
                int[] vector = new int[] { 0, 0, 0, 0, 0 };
                while (k >= 0)
                {
                    if (check(vector[k]) == 1)
                        vector[k] = 97;
                    else
                        vector[k]++;

                    if (k != 4 && check(vector[k]) == 0)
                        k++;
                    else
                        if (check(vector[k]) == 0)
                    {
                        if ((char)vector[0] == (char)vector[1] && (char)vector[0] == (char)vector[2] && (char)vector[0] == (char)vector[3] && (char)vector[0] == (char)vector[4])
                        {
                            Console.WriteLine((char)vector[0] + "" + (char)vector[1] + "" + (char)vector[2] + "" + (char)vector[3] + "" + (char)vector[4]);
                           
                        }
                        char[] list = new char[] { (char)vector[0], (char)vector[1], (char)vector[2], (char)vector[3], (char)vector[4] };
                        result = new string(list);
                        outputFile.WriteLine(result);
                        if (result == "zzzzz")
                            k = -1;

                    }

                    else
                        k--;
                }

            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}",
                  ts.Minutes, ts.Seconds);

            textBox1.Text = elapsedTime;
        }
        
        private void button5_Click(object sender, EventArgs e)
        {

            connection.Open();
            SqlTransaction sqlTran = connection.BeginTransaction();
            SqlCommand command = connection.CreateCommand();
            command.Transaction = sqlTran;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            makeList();
            DataTable dt = MakeDataTable();

            try
            {
                using (var bulkCopy = new SqlBulkCopy(connection.ConnectionString, SqlBulkCopyOptions.KeepIdentity))
                {


                    foreach (DataColumn col in dt.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }

                    bulkCopy.BulkCopyTimeout = 6000;
                    bulkCopy.DestinationTableName = "BulksPassword";

                    bulkCopy.WriteToServer(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("S-a facut roolback");
                try
                {
                    // Attempt to roll back the transaction.
                    sqlTran.Rollback();
                }
                catch (Exception exRollback)
                {

                    Console.WriteLine(exRollback.Message);
                }
            }
                stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}",
                 ts.Minutes, ts.Seconds);
                
            textBox1.Text = elapsedTime;
            connection.Close();
        }

        //Create dataTable
        private DataTable MakeDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Pass");
            foreach (var item in list)
            {
                var row = dt.NewRow();
                row["Pass"] = item;

                dt.Rows.Add(row);
            }
            return dt;


        }

        //Make a list of word(backtracking algorithm)
        private void makeList()
        {
            String result;

            int k = 0;
            int[] vector = new int[] { 0, 0, 0, 0, 0 };
            while (k >= 0)
            {
                if (check(vector[k]) == 1)
                    vector[k] = 97;
                else
                    vector[k]++;

                if (k != 4 && check(vector[k]) == 0)
                    k++;
                else
                    if (check(vector[k]) == 0)
                {


                    result = (char)vector[0] + " " + (char)vector[1] + " " + (char)vector[2] + " " + (char)vector[3] + " " + (char)vector[4];
                    list.Add(result);
                    if ((char)vector[0] == (char)vector[1] && (char)vector[0] == (char)vector[2] && (char)vector[0] == (char)vector[3] && (char)vector[0] == (char)vector[4])
                    {
                        Console.WriteLine(list.Count());
                        Console.WriteLine("Acum");
                    }
                }
                else
                    k--;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            insertList();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}",
                 ts.Minutes, ts.Seconds);
                
            textBox1.Text = elapsedTime;
        }

        //Make insert without bulk(just Password)
        public void insertList()
        {
            String result;
            connection.Open();
            int k = 0;
            int[] vector = new int[] { 0, 0, 0, 0, 0 };
            while (k >= 0)
            {
                if (check(vector[k]) == 1)
                    vector[k] = 97;
                else
                    vector[k]++;

                if (k != 4 && check(vector[k]) == 0)
                    k++;
                else
                    if (check(vector[k]) == 0)
                {


                    result = (char)vector[0] + " " + (char)vector[1] + " " + (char)vector[2] + " " + (char)vector[3] + " " + (char)vector[4];
                    adapter.InsertCommand = new SqlCommand("Insert Into BulksPassword VALUES(@pass)", connection);
                    adapter.InsertCommand.Parameters.Add("@pass", SqlDbType.VarChar).Value = result;

                    adapter.InsertCommand.ExecuteNonQuery();

                    if ((char)vector[0] == (char)vector[1] && (char)vector[0] == (char)vector[2] && (char)vector[0] == (char)vector[3] && (char)vector[0] == (char)vector[4])
                    {
                        Console.WriteLine(list.Count());
                        Console.WriteLine("Acum");
                    }
                }
                else
                    k--;
            }
            connection.Close();
        }


        //INSERT MULTIPLE ROWS IN BulksPassword(JUST PASSWORDS!)
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("InsertMultiple", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter p1 = cmd.CreateParameter();
                p1.DbType = System.Data.DbType.String;
                p1.ParameterName = "@pass";

              

                cmd.Parameters.Add(p1);
              

                for (int i = 5; i < 10; i++)
                {
                    p1.Value = "Ana" + i.ToString();
                  

                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
    }



