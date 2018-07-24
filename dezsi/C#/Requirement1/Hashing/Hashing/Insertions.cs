using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Hashing
{
    public class Insertions
    {

        public void CaptureMessages(SqlConnection connection) {
            connection.InfoMessage += new SqlInfoMessageEventHandler(myConnection_InfoMessage);

            void myConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
            {
                Console.WriteLine(e.Message);
            }
            

        }

        public enum Loading
        {
            InsertOneByOne,
            InsertTrans,
            MultipleRows,
            BulkInsert
        };

        List<string> arrangements;
        static string connectionString = "Data Source=DESKTOP-4KOH9RJ;Initial Catalog=Hashing;"
            + "Integrated Security=true;MultipleActiveResultSets=True";



        public Insertions(int passlen = 5) {
            arrangements = Generate.GeneratePermutations(passlen);
        }


        //inserts all
        public void InsertOneByOne() {

            foreach (string password in arrangements) {


                string md5hash = HashesClass.computeHash(password, "MD5");
                string sha1hash = HashesClass.computeHash(password, "SHA1");
                string sha2hash = HashesClass.computeHash(password, "SHA256");

                string queryString =
               "INSERT INTO Rainbow(password, md5hash, sha1hash, sha2hash) VALUES('" +
                   password + "', '" + md5hash + "', '" + sha1hash + "', '" + sha2hash + "')";
                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.ExecuteNonQuery();

                connection.Close();

            }
        }

        public void InsertMultipleRows(int step = 200) {
            for (int i = 0; i < arrangements.Count; i += step) {
                string queryString = "INSERT INTO Rainbow(password, md5hash, sha1hash, sha2hash) VALUES";
                try
                {
                    for (int j = 0; j < step; j++)
                    {
                        // Console.WriteLine(arrangements[i + j]);
                        // Console.WriteLine(queryString);

                        string md5hash = HashesClass.computeHash(arrangements[i + j], "MD5");
                        string sha1hash = HashesClass.computeHash(arrangements[i + j], "SHA1");
                        string sha2hash = HashesClass.computeHash(arrangements[i + j], "SHA256");

                        queryString += "('" + arrangements[i + j] + "', '" + md5hash + "', '" + sha1hash + "', '" + sha2hash + "'),";
                    }
                }
                catch (Exception) {
                    // index out of bound error
                }

                queryString = queryString.TrimEnd(',');
                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                SqlCommand command = new SqlCommand(queryString, connection);



                command.ExecuteNonQuery();

                Console.WriteLine("All " + step + " records are written to database.");

                connection.Close();

            }

        }
        /*
        public void getLastAddedElement() {
            
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string queryString = "select password from Rainbow where id = (select max(id) from Rainbow)";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.CommandText = queryString;

            Console.WriteLine(command.ExecuteScalar().ToString());

            connection.Close();

        }*/

        //inserts n rows
        public void InsertRowsTransaction(int n) {

            
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
          

            for (int j = 0; j < arrangements.Count - n; j += n)
            {

                 connection.Open();
  
                try
                {
                    SqlTransaction transaction = command.Connection.BeginTransaction("Transaction");
                    command.Transaction = transaction;
                    for (int i = j; i < j + n; i++)
                    {

                        string md5hash = HashesClass.computeHash(arrangements[i], "MD5");
                        string sha1hash = HashesClass.computeHash(arrangements[i], "SHA1");
                        string sha2hash = HashesClass.computeHash(arrangements[i], "SHA256");
                        string queryString =
                       "INSERT INTO Rainbow(password, md5hash, sha1hash, sha2hash) VALUES('" +
                           arrangements[i] + "', '" + md5hash + "', '" + sha1hash + "', '" + sha2hash + "')";
                        command.CommandText = queryString;
                        command.ExecuteNonQuery();
                       // Console.WriteLine(arrangements[i]);

                    }

                    transaction.Commit();
                    transaction.Dispose();
                    connection.Close();

                    Console.WriteLine("All " + n + " rows written to db");


                    // inside try
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                  //  command.Transaction.Rollback();

                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /*
        public void BulkInsertXml(int bufferSize) {
           
            bufferSize = 5;


            XmlWriter xmlWriter = XmlWriter.Create("rainbow.xml");
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("hashings");
            int i = 0;
            int count = arrangements.Count();
            while (i == 0) {

                for (int j = i; j < i+ bufferSize; j++) {
                    string md5hash = HashesClass.computeHash(arrangements[j], "MD5");
                    string sha1hash = HashesClass.computeHash(arrangements[j], "SHA1");
                    string sha2hash = HashesClass.computeHash(arrangements[j], "SHA256");
                    xmlWriter.WriteStartElement("hashing");
                    xmlWriter.WriteAttributeString("password", arrangements[j]);
                    xmlWriter.WriteAttributeString("md5hash", md5hash);
                    xmlWriter.WriteAttributeString("sha1hash", sha1hash);
                    xmlWriter.WriteAttributeString("sha2hash", sha2hash);
                    xmlWriter.WriteEndElement();
                   
                }

                i += bufferSize;
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();

                var doc = XDocument.Load("rainbow.xml");

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("password"));
                dt.Columns.Add(new DataColumn("md5hash"));
                dt.Columns.Add(new DataColumn("sha1hash"));
                dt.Columns.Add(new DataColumn("sha2hash"));
                doc.Descendants("hashings").ToList().ForEach(e=>Console.WriteLine(e.Attribute("password").Value));

                var rows = doc.Descendants("hashing").Select(el => {
                    DataRow row = dt.NewRow();
                    row["password"] = el.Attribute("password").Value;
                    row["md5hash"] = el.Attribute("md5hash").Value;
                    row["sha1hash"] = el.Attribute("sha1hash").Value;
                    row["sha2hash"] = el.Attribute("sha2hash").Value;
                    return row;
                });
                dt.Rows.Add(rows);
                Console.WriteLine((dt.Rows[0]["password"]).ToString());

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                using (var bcp = new SqlBulkCopy(connection))
                {


                    SqlBulkCopyColumnMapping mapPass =
                       new SqlBulkCopyColumnMapping("password", "password");
                    bcp.ColumnMappings.Add(mapPass);

                    SqlBulkCopyColumnMapping mapMd5 =
                        new SqlBulkCopyColumnMapping("md5hash", "md5hash");
                    bcp.ColumnMappings.Add(mapMd5);

                    SqlBulkCopyColumnMapping mapSha1 =
                        new SqlBulkCopyColumnMapping("sha1hash", "sha1hash");
                    bcp.ColumnMappings.Add(mapSha1);

                    SqlBulkCopyColumnMapping mapSha2 =
                    new SqlBulkCopyColumnMapping("sha2hash", "sha2hash");
                    bcp.ColumnMappings.Add(mapSha2);


                    bcp.BatchSize = bufferSize;
                    bcp.DestinationTableName = "Rainbow";
                    bcp.WriteToServer(dt);
                }

                connection.Close();

    


            }
        }
        */
        public void BulkInsert() {


            DataTable dt = new DataTable();
            dt.Columns.Add("password");
            dt.Columns.Add("md5hash");
            dt.Columns.Add("sha1hash");
            dt.Columns.Add("sha2hash");



            for (int i = 0; i < arrangements.Count; i++) {
                DataRow row = dt.NewRow();
                row["password"] = arrangements[i];
                row["md5hash"] = HashesClass.computeHash(arrangements[i], "MD5");
                row["sha1hash"] = HashesClass.computeHash(arrangements[i], "SHA1");
                row["sha2hash"] = HashesClass.computeHash(arrangements[i], "SHA256");
                dt.Rows.Add(row);
                // Console.WriteLine(arrangements[i]);
            }
            Console.WriteLine("finished creating data table");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (var bcp = new SqlBulkCopy(connection)) {


                SqlBulkCopyColumnMapping mapPass =
                   new SqlBulkCopyColumnMapping("password", "password");
                bcp.ColumnMappings.Add(mapPass);

                SqlBulkCopyColumnMapping mapMd5 =
                    new SqlBulkCopyColumnMapping("md5hash", "md5hash");
                bcp.ColumnMappings.Add(mapMd5);

                SqlBulkCopyColumnMapping mapSha1 =
                    new SqlBulkCopyColumnMapping("sha1hash", "sha1hash");
                bcp.ColumnMappings.Add(mapSha1);

                SqlBulkCopyColumnMapping mapSha2 =
                new SqlBulkCopyColumnMapping("sha2hash", "sha2hash");
                bcp.ColumnMappings.Add(mapSha2);


                bcp.BatchSize = 100;
                bcp.DestinationTableName = "Rainbow";
                bcp.WriteToServer(dt);

            }

            connection.Close();


        }


        public void LoadIntoDatabase(Loading type) {
            if (type == Loading.InsertOneByOne) {
                InsertOneByOne();
            }
            else if (type == Loading.InsertTrans) {

                InsertRowsTransaction(1000);

            } else if (type == Loading.MultipleRows) {

                InsertMultipleRows();

            } else {

                BulkInsert();

            }
        }

    } //Insertions class
} //Hashing namespace
