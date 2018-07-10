using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hash_RevHash
{
    class Program
    {
        public static List<string> passwords;
        public static string currentPassword;

        public static void GeneratePasswords(int i, int n)
        {
            if (i == n) 
                passwords.Add(currentPassword);
            else if (i<n)
            {
                for (char c = 'a'; c <= 'z'; c++)
                {
                    currentPassword += c;
                    GeneratePasswords(i + 1, n);
                    currentPassword=currentPassword.Remove(currentPassword.Length - 1);
                }
            }
        }

        private static void AddPasswordsToDatabaseSingleLine()
        {
            Connection c = new Connection();
            foreach(string s in passwords)
            {
                c.InsertData(s);
            }
        }
        
        private static void AddPasswordsToDatabaseMultipleLines()
        {
            Connection c = new Connection();
            c.insertLines(passwords,0,passwords.Count/4);
            Console.WriteLine("primul sfert");
            c.insertLines(passwords, passwords.Count / 4, passwords.Count / 2);
            Console.WriteLine("al doilea sfert");
            c.insertLines(passwords, passwords.Count / 2, passwords.Count / 2 + passwords.Count / 4);
            Console.WriteLine("al treilea sfert");
            c.insertLines(passwords, passwords.Count / 2 + passwords.Count / 4, passwords.Count);
            
        }

        private static void AddPasswordsToDatabaseBulk()
        {
            Connection c = new Connection();
            c.insertBulk(passwords, 0, passwords.Count / 4);
            Console.WriteLine("primul sfert");
            c.insertBulk(passwords, passwords.Count / 4, passwords.Count / 2);
            Console.WriteLine("al doilea sfert");
            c.insertBulk(passwords, passwords.Count / 2, passwords.Count / 2 + passwords.Count / 4);
            Console.WriteLine("al treilea sfert");
            c.insertBulk(passwords, passwords.Count / 2 + passwords.Count / 4, passwords.Count);
        }

        private static void CleanDatabase()
        {
            Connection c = new Connection();
            c.CleanDatabase();
        }

        private static void TestInsertTimes()
        {

            Stopwatch stopWatch = new Stopwatch();
            TimeSpan ts;
            string elapsedTime;

            Console.WriteLine("Cleaning Database");
            stopWatch.Reset();
            stopWatch.Start();
            CleanDatabase();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Cleaning database takes " + elapsedTime);




            Console.WriteLine("Single Line Insert");
            stopWatch.Reset();
            stopWatch.Start();
            AddPasswordsToDatabaseSingleLine();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Single line insert takes " + elapsedTime);
            
            stopWatch.Reset();
            stopWatch.Start();
            CleanDatabase();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds,  ts.Milliseconds / 10);
            Console.WriteLine("Cleaning database takes " + elapsedTime);


            Console.WriteLine("Multiple Lines Insert");
            stopWatch.Reset();
            stopWatch.Start();
            AddPasswordsToDatabaseMultipleLines();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Multiple line insert takes " + elapsedTime);
            
            Console.WriteLine("Cleaning Database");
            stopWatch.Reset();
            stopWatch.Start();
            CleanDatabase();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Cleaning database takes " + elapsedTime);


            Console.WriteLine("Bulk Insert");
            stopWatch.Reset();
            stopWatch.Start();
            AddPasswordsToDatabaseBulk();
            stopWatch.Stop(); ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Bulk insert takes " + elapsedTime);
        }

        public static string RevHash(string hash)
        {
            Connection c = new Connection();
            return c.GetPasswordFromHash(hash);
        }

        public static void TestReverseHashTime()
        {
            Stopwatch stopWatch = new Stopwatch();
            TimeSpan ts;
            string elapsedTime;
            stopWatch.Reset();
            stopWatch.Start();
            Console.WriteLine(RevHash("594f803b380a41396ed63dca39503542"));
            stopWatch.Stop(); ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("Get takes " + elapsedTime);
        }

        static void Main(string[] args)
        {
            

            passwords = new List<string>();
            int n = 5;
            Console.WriteLine("generating passwords");
            GeneratePasswords(0, n);
            Console.WriteLine("writing to database");
            TestInsertTimes();
            TestReverseHashTime();
            
        }
    }
}
