using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HashDehash
{
    public struct Passes
    {
        public string password;
        public char[] MD5;
        public char[] SHA1;
        public char[] SHA256;
        public char[] SHA384;
        public char[] SHA512;
    }

    class Program
    {
        private static ArrayList passwords = new ArrayList();
        private static ArrayList passwordsSynchronized = ArrayList.Synchronized(passwords);
        private static readonly SqlConnection conn = new SqlConnection("Data Source=DESKTOP-BRM0244;Initial Catalog=HashDB;Integrated Security=True");

        static void Main(string[] args)
        {
            ConsoleKeyInfo op;

            do
            {
                Console.Clear();
                Console.WriteLine("Hello there!");
                Console.WriteLine("Choose your option:");
                Console.WriteLine("1 - Generate passwords and hashes");
                Console.WriteLine("2 - Serialize (needs the passwords to be generated)");
                Console.WriteLine("3 - Deserialize (recommended to do from RAMDisk)");
                Console.WriteLine("4 - Write to DB line by line");
                Console.WriteLine("5 - Write to DB by multiple lines");
                Console.WriteLine("6 - Write to DB in bulk");
                Console.WriteLine("7 - Automatic benchmark");
                Console.WriteLine("8 - Clear DB");
                Console.WriteLine("9 - Reverse Hash");
                Console.WriteLine("ESC - Exit");

                op = Console.ReadKey();
                Console.WriteLine("");

                switch (op.KeyChar)
                {
                    case '1': generatePasswords(); break;
                    case '2': Serialize(); break;
                    case '3': Deserialize(); break;
                    case '4':
                        Console.WriteLine("EF? (yes = 1, no = 0)");
                        ConsoleKeyInfo ef = Console.ReadKey();
                        if (ef.KeyChar == '0')
                        {
                            SendLineByLine();
                        }
                        else
                        {
                            SendLineByLineEF();
                        }
                        Console.ReadKey();
                        break;
                    case '5': SendMultipleLines(1000); Console.ReadKey(); break;
                    case '6': SendBulk(); Console.ReadKey(); break;
                    case '7':
                        Deserialize();
                        Console.WriteLine("DB clear:");
                        EmptyTable();
                        Console.WriteLine("Line by line:");
                        SendLineByLine();

                        Console.WriteLine("Reverse hash 207595ac89a3f77469c0c2df10da459f MD5 (tzzyt): ");
                        ReverseHash("MD5", "207595ac89a3f77469c0c2df10da459f");

                        Console.WriteLine("Reverse hash 20a31c0625c9a3b1a12e5466bcd18701fffb7ae6 SHA1 (tzzww): ");
                        ReverseHash("SHA1", "20a31c0625c9a3b1a12e5466bcd18701fffb7ae6");

                        Console.WriteLine("Reverse hash 308cb12d5caf880e12054d9c52786aee36baa91e6e9f3f92825c43de4fa38c5e SHA256 (tzzsv): ");
                        ReverseHash("SHA256", "308cb12d5caf880e12054d9c52786aee36baa91e6e9f3f92825c43de4fa38c5e");

                        Console.WriteLine("Reverse hash f05e30a5d7c4e01b3a79906e1a01cbc695c03a0986c7fbe33f57a9e8328e129255c380ccd171446bc7aa8f1233ebd8b7 SHA384 (tzzrf): ");
                        ReverseHash("SHA384", "f05e30a5d7c4e01b3a79906e1a01cbc695c03a0986c7fbe33f57a9e8328e129255c380ccd171446bc7aa8f1233ebd8b7");

                        Console.WriteLine("Reverse hash d0b80d9dc004e810bac751edb32ac0fdd9f98eb413359321f9a75f64a3790f921feed6fdc89a67f6955f1f2fd5099c64a7231197b688ad389493c35712fa8fe7 SHA512 (tzzpf): ");
                        ReverseHash("SHA512", "d0b80d9dc004e810bac751edb32ac0fdd9f98eb413359321f9a75f64a3790f921feed6fdc89a67f6955f1f2fd5099c64a7231197b688ad389493c35712fa8fe7");

                        Console.WriteLine("DB clear:");
                        EmptyTable();

                        Console.WriteLine("Multiple lines (1000):");
                        SendMultipleLines(1000);

                        Console.WriteLine("DB clear:");
                        EmptyTable();

                        Console.WriteLine("Bulk:");
                        SendBulk();

                        Console.WriteLine("Bulk:");
                        SendBulk();

                        Console.WriteLine("DB clear:");
                        EmptyTable();
                        Console.ReadKey();
                        break;
                    case '8': EmptyTable(); Console.ReadKey(); break;
                    case '9':
                        Console.WriteLine("1 - MD5");
                        Console.WriteLine("2 - SHA1");
                        Console.WriteLine("3 - SHA256");
                        Console.WriteLine("4 - SHA384");
                        Console.WriteLine("5 - SHA512");


                        ConsoleKeyInfo hashtype = Console.ReadKey();
                        Console.WriteLine("");

                        Console.WriteLine("Hash:");
                        string hash = Console.ReadLine();

                        switch (hashtype.KeyChar)
                        {
                            case '1': ReverseHash("MD5", hash); break;
                            case '2': ReverseHash("SHA1", hash); break;
                            case '3': ReverseHash("SHA256", hash); break;
                            case '4': ReverseHash("SHA384", hash); break;
                            case '5': ReverseHash("SHA512", hash); break;
                        }

                        Console.ReadKey();
                        break;
                    default: break;
                }

            }
            while (op.Key != ConsoleKey.Escape);
        }

        private static void generatePasswords()
        {
            /*
            * MD5 32
            * SHA1 40
            * SHA256 64
            * SHA384 96
            * SHA512 128
            */

            Thread t1 = new Thread(() =>
            {
                char[] charArr = new char[5];
                for (char c1 = 'a'; c1 <= 'd'; c1++) //a=>d
                {
                    for (char c2 = 'a'; c2 <= 'z'; c2++)
                    {
                        for (char c3 = 'a'; c3 <= 'z'; c3++)
                        {
                            for (char c4 = 'a'; c4 <= 'z'; c4++)
                            {
                                for (char c5 = 'a'; c5 <= 'z'; c5++)
                                {
                                    charArr[0] = c1;
                                    charArr[1] = c2;
                                    charArr[2] = c3;
                                    charArr[3] = c4;
                                    charArr[4] = c5;

                                    string pass = String.Concat(charArr);

                                    if (c1 == c2 && c2 == c3 && c3 == c4 && c4 == c5)
                                    {
                                        Console.WriteLine(pass);
                                    }

                                    Passes p = new Passes();
                                    p.password = pass;
                                    p.MD5 = MD5Hash(pass);
                                    p.SHA1 = SHA1Hash(pass);
                                    p.SHA256 = SHA256Hash(pass);
                                    p.SHA384 = SHA384Hash(pass);
                                    p.SHA512 = SHA512Hash(pass);

                                    passwordsSynchronized.Add(p);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Thread1 done");
            });
            Thread t2 = new Thread(() =>
            {
                char[] charArr = new char[5];
                for (char c1 = 'e'; c1 <= 'h'; c1++)
                {
                    for (char c2 = 'a'; c2 <= 'z'; c2++)
                    {
                        for (char c3 = 'a'; c3 <= 'z'; c3++)
                        {
                            for (char c4 = 'a'; c4 <= 'z'; c4++)
                            {
                                for (char c5 = 'a'; c5 <= 'z'; c5++)
                                {
                                    charArr[0] = c1;
                                    charArr[1] = c2;
                                    charArr[2] = c3;
                                    charArr[3] = c4;
                                    charArr[4] = c5;

                                    string pass = String.Concat(charArr);

                                    if (c1 == c2 && c2 == c3 && c3 == c4 && c4 == c5)
                                    {
                                        Console.WriteLine(pass);
                                    }


                                    Passes p = new Passes();
                                    p.password = pass;
                                    p.MD5 = MD5Hash(pass);
                                    p.SHA1 = SHA1Hash(pass);
                                    p.SHA256 = SHA256Hash(pass);
                                    p.SHA384 = SHA384Hash(pass);
                                    p.SHA512 = SHA512Hash(pass);

                                    passwordsSynchronized.Add(p);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Thread2 done");
            });
            Thread t3 = new Thread(() =>
            {
                char[] charArr = new char[5];
                for (char c1 = 'i'; c1 <= 'l'; c1++)
                {
                    for (char c2 = 'a'; c2 <= 'z'; c2++)
                    {
                        for (char c3 = 'a'; c3 <= 'z'; c3++)
                        {
                            for (char c4 = 'a'; c4 <= 'z'; c4++)
                            {
                                for (char c5 = 'a'; c5 <= 'z'; c5++)
                                {
                                    charArr[0] = c1;
                                    charArr[1] = c2;
                                    charArr[2] = c3;
                                    charArr[3] = c4;
                                    charArr[4] = c5;

                                    string pass = String.Concat(charArr);

                                    if (c1 == c2 && c2 == c3 && c3 == c4 && c4 == c5)
                                    {
                                        Console.WriteLine(pass);
                                    }

                                    Passes p = new Passes();
                                    p.password = pass;
                                    p.MD5 = MD5Hash(pass);
                                    p.SHA1 = SHA1Hash(pass);
                                    p.SHA256 = SHA256Hash(pass);
                                    p.SHA384 = SHA384Hash(pass);
                                    p.SHA512 = SHA512Hash(pass);

                                    passwordsSynchronized.Add(p);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Thread3 done");
            });
            Thread t4 = new Thread(() =>
            {
                char[] charArr = new char[5];
                for (char c1 = 'm'; c1 <= 'p'; c1++)
                {
                    for (char c2 = 'a'; c2 <= 'z'; c2++)
                    {
                        for (char c3 = 'a'; c3 <= 'z'; c3++)
                        {
                            for (char c4 = 'a'; c4 <= 'z'; c4++)
                            {
                                for (char c5 = 'a'; c5 <= 'z'; c5++)
                                {
                                    charArr[0] = c1;
                                    charArr[1] = c2;
                                    charArr[2] = c3;
                                    charArr[3] = c4;
                                    charArr[4] = c5;

                                    string pass = String.Concat(charArr);

                                    if (c1 == c2 && c2 == c3 && c3 == c4 && c4 == c5)
                                    {
                                        Console.WriteLine(pass);
                                    }

                                    Passes p = new Passes();
                                    p.password = pass;
                                    p.MD5 = MD5Hash(pass);
                                    p.SHA1 = SHA1Hash(pass);
                                    p.SHA256 = SHA256Hash(pass);
                                    p.SHA384 = SHA384Hash(pass);
                                    p.SHA512 = SHA512Hash(pass);

                                    passwordsSynchronized.Add(p);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Thread4 done");
            });
            Thread t5 = new Thread(() =>
            {
                char[] charArr = new char[5];
                for (char c1 = 'q'; c1 <= 't'; c1++)
                {
                    for (char c2 = 'a'; c2 <= 'z'; c2++)
                    {
                        for (char c3 = 'a'; c3 <= 'z'; c3++)
                        {
                            for (char c4 = 'a'; c4 <= 'z'; c4++)
                            {
                                for (char c5 = 'a'; c5 <= 'z'; c5++)
                                {
                                    charArr[0] = c1;
                                    charArr[1] = c2;
                                    charArr[2] = c3;
                                    charArr[3] = c4;
                                    charArr[4] = c5;

                                    string pass = String.Concat(charArr);

                                    if (c1 == c2 && c2 == c3 && c3 == c4 && c4 == c5)
                                    {
                                        Console.WriteLine(pass);
                                    }

                                    Passes p = new Passes();
                                    p.password = pass;
                                    p.MD5 = MD5Hash(pass);
                                    p.SHA1 = SHA1Hash(pass);
                                    p.SHA256 = SHA256Hash(pass);
                                    p.SHA384 = SHA384Hash(pass);
                                    p.SHA512 = SHA512Hash(pass);

                                    passwordsSynchronized.Add(p);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Thread5 done");
            });
            Thread t6 = new Thread(() =>
            {
                char[] charArr = new char[5];
                for (char c1 = 'u'; c1 <= 'x'; c1++)
                {
                    for (char c2 = 'a'; c2 <= 'z'; c2++)
                    {
                        for (char c3 = 'a'; c3 <= 'z'; c3++)
                        {
                            for (char c4 = 'a'; c4 <= 'z'; c4++)
                            {
                                for (char c5 = 'a'; c5 <= 'z'; c5++)
                                {
                                    charArr[0] = c1;
                                    charArr[1] = c2;
                                    charArr[2] = c3;
                                    charArr[3] = c4;
                                    charArr[4] = c5;

                                    string pass = String.Concat(charArr);

                                    if (c1 == c2 && c2 == c3 && c3 == c4 && c4 == c5)
                                    {
                                        Console.WriteLine(pass);
                                    }

                                    Passes p = new Passes();
                                    p.password = pass;
                                    p.MD5 = MD5Hash(pass);
                                    p.SHA1 = SHA1Hash(pass);
                                    p.SHA256 = SHA256Hash(pass);
                                    p.SHA384 = SHA384Hash(pass);
                                    p.SHA512 = SHA512Hash(pass);

                                    passwordsSynchronized.Add(p);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Thread6 done");
            });
            Thread t7 = new Thread(() =>
            {
                char[] charArr = new char[5];
                for (char c1 = 'y'; c1 <= 'z'; c1++)
                {
                    for (char c2 = 'a'; c2 <= 'z'; c2++)
                    {
                        for (char c3 = 'a'; c3 <= 'z'; c3++)
                        {
                            for (char c4 = 'a'; c4 <= 'z'; c4++)
                            {
                                for (char c5 = 'a'; c5 <= 'z'; c5++)
                                {
                                    charArr[0] = c1;
                                    charArr[1] = c2;
                                    charArr[2] = c3;
                                    charArr[3] = c4;
                                    charArr[4] = c5;

                                    string pass = String.Concat(charArr);

                                    if (c1 == c2 && c2 == c3 && c3 == c4 && c4 == c5)
                                    {
                                        Console.WriteLine(pass);
                                    }

                                    Passes p = new Passes();
                                    p.password = pass;
                                    p.MD5 = MD5Hash(pass);
                                    p.SHA1 = SHA1Hash(pass);
                                    p.SHA256 = SHA256Hash(pass);
                                    p.SHA384 = SHA384Hash(pass);
                                    p.SHA512 = SHA512Hash(pass);

                                    passwordsSynchronized.Add(p);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Thread7 done");
            });

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
            t5.Join();
            t6.Join();
            t7.Join();
        }

        private static void Serialize()
        {
            Stream stream = new FileStream("MyFile.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            StreamWriter sw = new StreamWriter(stream);
            foreach (Passes pc in passwords)
            {
                sw.WriteLine(ToCharArray(pc));
            }
            sw.Close();
            stream.Close();
        }

        private static void Deserialize()
        {
            passwords.Clear();
            Stream stream = new FileStream("MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(stream);
            string pc = sr.ReadLine();
            while (pc != null)
            {
                string[] pcTokens = pc.Split(' ');
                Passes p = new Passes();
                p.password = pcTokens[0];
                p.MD5 = pcTokens[1].ToCharArray();
                p.SHA1 = pcTokens[2].ToCharArray();
                p.SHA256 = pcTokens[3].ToCharArray();
                p.SHA384 = pcTokens[4].ToCharArray();
                p.SHA512 = pcTokens[5].ToCharArray();
                passwords.Add(p);
                pc = sr.ReadLine();
            }
            sr.Close();
            stream.Close();
        }

        private static char[] ToCharArray(Passes p)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(p.password);
            sb.Append(" ");
            sb.Append(p.MD5);
            sb.Append(" ");
            sb.Append(p.SHA1);
            sb.Append(" ");
            sb.Append(p.SHA256);
            sb.Append(" ");
            sb.Append(p.SHA384);
            sb.Append(" ");
            sb.Append(p.SHA512);


            return sb.ToString().ToCharArray();
        }

        private static char[] MD5Hash(string password)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider MD5provider = new MD5CryptoServiceProvider();
            byte[] bytes = MD5provider.ComputeHash(new UTF8Encoding().GetBytes(password));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString().ToCharArray();
        }

        private static char[] SHA1Hash(string password)
        {
            StringBuilder hash = new StringBuilder();
            SHA1CryptoServiceProvider SHA1provider = new SHA1CryptoServiceProvider();
            byte[] bytes = SHA1provider.ComputeHash(new UTF8Encoding().GetBytes(password));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString().ToCharArray();
        }

        private static char[] SHA256Hash(string password)
        {
            StringBuilder hash = new StringBuilder();
            SHA256CryptoServiceProvider SHA256provider = new SHA256CryptoServiceProvider();
            byte[] bytes = SHA256provider.ComputeHash(new UTF8Encoding().GetBytes(password));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString().ToCharArray();
        }

        private static char[] SHA384Hash(string password)
        {
            StringBuilder hash = new StringBuilder();
            SHA384CryptoServiceProvider SHA384provider = new SHA384CryptoServiceProvider();
            byte[] bytes = SHA384provider.ComputeHash(new UTF8Encoding().GetBytes(password));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString().ToCharArray();
        }

        private static char[] SHA512Hash(string password)
        {
            StringBuilder hash = new StringBuilder();
            SHA512CryptoServiceProvider SHA256provider = new SHA512CryptoServiceProvider();
            byte[] bytes = SHA256provider.ComputeHash(new UTF8Encoding().GetBytes(password));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString().ToCharArray();
        }

        private static void SendLineByLine()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (Passes p in passwords)
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO HashRainbow (Password, MD5, SHA1, SHA256, SHA384, SHA512) VALUES (@pass, @md5, @sha1, @sha256, @sha384, @sha512)");
                    cmd.Parameters.Add("@pass", SqlDbType.Char, 5).Value = p.password;
                    cmd.Parameters.Add("@md5", SqlDbType.Char, 32).Value = p.MD5;
                    cmd.Parameters.Add("@sha1", SqlDbType.Char, 40).Value = p.SHA1;
                    cmd.Parameters.Add("@sha256", SqlDbType.Char, 64).Value = p.SHA256;
                    cmd.Parameters.Add("@sha384", SqlDbType.Char, 96).Value = p.SHA384;
                    cmd.Parameters.Add("@sha512", SqlDbType.Char, 128).Value = p.SHA512;
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException e) { Console.WriteLine("conn failed to open. err: " + e.Message); }
                finally
                {
                    conn.Close();
                }
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsed = String.Format("{0:00} : {1:00} : {2:00} : {3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine(elapsed);
        }

        private static void SendMultipleLines(int count)
        {
            int LowerLimit = 0;
            int UpperLimit = count;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            bool finish = false;
            while (true)
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO HashRainbow (Password, MD5, SHA1, SHA256, SHA384, SHA512) VALUES (@pass, @md5, @sha1, @sha256, @sha384, @sha512)");
                    cmd.Connection = conn;
                    cmd.Transaction = conn.BeginTransaction("transaction");
                    for (int i = LowerLimit; i < UpperLimit; i++)
                    {
                        cmd.Parameters.Add("@pass", SqlDbType.Char, 5).Value = ((Passes)passwords[i]).password;
                        cmd.Parameters.Add("@md5", SqlDbType.Char, 32).Value = ((Passes)passwords[i]).MD5;
                        cmd.Parameters.Add("@sha1", SqlDbType.Char, 40).Value = ((Passes)passwords[i]).SHA1;
                        cmd.Parameters.Add("@sha256", SqlDbType.Char, 64).Value = ((Passes)passwords[i]).SHA256;
                        cmd.Parameters.Add("@sha384", SqlDbType.Char, 96).Value = ((Passes)passwords[i]).SHA384;
                        cmd.Parameters.Add("@sha512", SqlDbType.Char, 128).Value = ((Passes)passwords[i]).SHA512;
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    cmd.Transaction.Commit();
                }
                catch (SqlException e) { Console.WriteLine("conn failed to open. err: " + e.Message); }

                finally
                {
                    conn.Close();
                }

                if (finish) break;

                LowerLimit = UpperLimit;
                UpperLimit += count;
                if (UpperLimit > passwords.Count)
                {
                    UpperLimit = passwords.Count;
                    finish = true;
                }
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsed = String.Format("{0:00} : {1:00} : {2:00} : {3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine(elapsed);
        }

        private static void SendBulk()
        {
            passwords.Clear();
            int UpperLimit = passwords.Count;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            sw.Stop();
            try
            {
                conn.Open();
                int count = 0;

                DataTable dt = new DataTable();
                dt.Columns.Add("Password", typeof(string));
                dt.Columns.Add("MD5", typeof(char[]));
                dt.Columns.Add("SHA1", typeof(char[]));
                dt.Columns.Add("SHA256", typeof(char[]));
                dt.Columns.Add("SHA384", typeof(char[]));
                dt.Columns.Add("SHA512", typeof(char[]));

                SqlBulkCopy bk = new SqlBulkCopy(conn);

                SqlBulkCopyColumnMapping cmpass = new SqlBulkCopyColumnMapping("Password", "Password");
                SqlBulkCopyColumnMapping cmmd5 = new SqlBulkCopyColumnMapping("MD5", "MD5");
                SqlBulkCopyColumnMapping cmsha1 = new SqlBulkCopyColumnMapping("SHA1", "SHA1");
                SqlBulkCopyColumnMapping cmsha2 = new SqlBulkCopyColumnMapping("SHA256", "SHA256");
                SqlBulkCopyColumnMapping cmsha3 = new SqlBulkCopyColumnMapping("SHA384", "SHA384");
                SqlBulkCopyColumnMapping cmsha5 = new SqlBulkCopyColumnMapping("SHA512", "SHA512");

                bk.ColumnMappings.Add(cmpass);
                bk.ColumnMappings.Add(cmmd5);
                bk.ColumnMappings.Add(cmsha1);
                bk.ColumnMappings.Add(cmsha2);
                bk.ColumnMappings.Add(cmsha3);
                bk.ColumnMappings.Add(cmsha5);

                bk.DestinationTableName = "HashRainbow";
                bk.BatchSize = 10000;

                Stream stream = new FileStream("MyFile.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(stream);
                string pc = sr.ReadLine();
                while (pc != null)
                {
                    string[] pcTokens = pc.Split(' ');
                    Passes p = new Passes();
                    p.password = pcTokens[0];
                    p.MD5 = pcTokens[1].ToCharArray();
                    p.SHA1 = pcTokens[2].ToCharArray();
                    p.SHA256 = pcTokens[3].ToCharArray();
                    p.SHA384 = pcTokens[4].ToCharArray();
                    p.SHA512 = pcTokens[5].ToCharArray();
                    DataRow dr = dt.NewRow();
                    dr["Password"] = p.password;
                    dr["MD5"] = p.MD5.Take(32).ToArray();
                    dr["SHA1"] = p.SHA1.Take(40).ToArray();
                    dr["SHA256"] = p.SHA256.Take(64).ToArray();
                    dr["SHA384"] = p.SHA384.Take(96).ToArray();
                    dr["SHA512"] = p.SHA512.Take(128).ToArray();
                    dt.Rows.Add(dr);
                    count++;
                    pc = sr.ReadLine();

                    if (count > 3960458 || pc == null)
                    {
                        count = 0;
                        sw.Start();
                        bk.WriteToServer(dt);
                        sw.Stop();
                        dt.Rows.Clear();
                        Console.WriteLine("O treime");
                    }
                }
                sr.Close();
                stream.Close();

            }
            catch (SqlException e) { Console.WriteLine("conn failed to open. err: " + e.Message); }

            finally
            {
                conn.Close();
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsed = String.Format("{0:00} : {1:00} : {2:00} : {3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine(elapsed);
        }

        private static void EmptyTable()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("TRUNCATE TABLE HashRainbow");
                cmd.Connection = conn;
                cmd.CommandTimeout = 1000;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e) { Console.WriteLine("conn failed to open. err: " + e.Message); }
            finally
            {
                conn.Close();
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsed = String.Format("{0:00} : {1:00} : {2:00} : {3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine(elapsed);
        }

        private static void ReverseHash(string HashType, string HashValue)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Password FROM HashRainbow WHERE " + HashType + " = '" + HashValue + "'");
                Console.WriteLine(cmd.CommandText);
                cmd.Connection = conn;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("Pass is : {0}", reader[0]);
                }
                reader.Close();
            }
            catch (SqlException e) { Console.WriteLine("conn failed to open. err: " + e.Message); }
            finally
            {
                conn.Close();
            }
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsed = String.Format("{0:00} : {1:00} : {2:00} : {3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine(elapsed);
        }

        private static void SendLineByLineEF()
        {
            Stopwatch sw = new Stopwatch();
            HashDBEntities1 repo = new HashDBEntities1();
            foreach (Passes p in passwords)
            {
                HashRainbow row = new HashRainbow();
                row.Password = p.password;
                row.SHA1 = p.SHA1.ToString();
                row.SHA256 = p.SHA256.ToString();
                row.SHA384 = p.SHA384.ToString();
                row.SHA512 = p.SHA512.ToString();

                sw.Start();
                repo.HashRainbows.Add(row);
                sw.Stop();
            }

            TimeSpan ts = sw.Elapsed;
            string elapsed = String.Format("{0:00} : {1:00} : {2:00} : {3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Console.WriteLine(elapsed);
        }

    }
}
