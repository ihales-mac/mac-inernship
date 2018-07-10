using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.BulkInsert.Exceptions;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.MappingAPI;

namespace EntityFrameworkRainbow2
{
    class Program
    {
        //THE 2 FUNCTIONS FROM(Check and printSolution) FOR BACKTRACKING
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
        public static void insertJustPassword()
        {
            RainbowEntities rainbowEntities = new RainbowEntities();
            BulksPassword bulksPassword = new BulksPassword();
            String result;
            int k = 0;
            int[] vector = new int[] { 0, 0, 0, 0, 0 };

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();




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
                    char[] list = new char[] { (char)vector[0], (char)vector[1], (char)vector[2], (char)vector[3], (char)vector[4] };
                    result = new string(list);
                    if ((char)vector[0] == (char)vector[1] && (char)vector[0] == (char)vector[2] && (char)vector[0] == (char)vector[3] && (char)vector[0] == (char)vector[4])
                    {

                        Console.WriteLine(result);

                    }
                  //  Console.WriteLine(result);
                   
                    bulksPassword.Pass = result;
                    rainbowEntities.BulksPasswords.Add(bulksPassword);
                    

                    if (result == "zzzzz")
                        k = -1;
                }



                else
                    k--;
            }



            rainbowEntities.SaveChanges();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            Console.WriteLine("Timpul(Minute:Secunde)" + elapsedTime);
        }

        public static void deleteRecordsFromBulksPassword()
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var context = new RainbowEntities();
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [BulksPassword]");

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            Console.WriteLine("Timpul(Minute:Secunde)" + elapsedTime);
        }

        public static void BulkInsertIntoBulksPassword()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var context = new RainbowEntities();

            String result;
            int k = 0;
            int[] vector = new int[] { 0, 0, 0, 0, 0 };
            List<BulksPassword> list = new List<BulksPassword>();
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
                    char[] list2 = new char[] { (char)vector[0], (char)vector[1], (char)vector[2], (char)vector[3], (char)vector[4] };
                    result = new string(list2);
                    if ((char)vector[0] == (char)vector[1] && (char)vector[0] == (char)vector[2] && (char)vector[0] == (char)vector[3] && (char)vector[0] == (char)vector[4])
                    {

                        Console.WriteLine(result);

                    }
                    //  Console.WriteLine(result);

                    BulksPassword obj = new BulksPassword();
                    obj.Pass = result;
                    list.Add(obj);

                    if (result.Equals("zzzzz"))
                        k = -1;
                }



                else
                    k--;
            }
            context.BulkInsert(list);
            context.SaveChanges();
                
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            Console.WriteLine("Timpul(Minute:Secunde)" + elapsedTime);
        }

        public static void Search(String type,String pass)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var context = new RainbowEntities();
            if (type == "MD5")
            {
                var blogs = from password in context.Passes
                            where password.MD5 == pass
                            select password.Pass1;



                foreach (var item in blogs)
                    Console.WriteLine(item);
            }
            else
                if (type == "SHA1")
            {
                var blogs = from password in context.Passes
                            where password.SHA1 == pass
                            select password.Pass1;



                foreach (var item in blogs)
                    Console.WriteLine(item);
            }
            else
                if (type == "SHA2")
            {
                var blogs = from password in context.Passes
                            where password.SHA2 == pass
                            select password.Pass1;



                foreach (var item in blogs)
                    Console.WriteLine(item);
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            Console.WriteLine("Timpul(Minute:Secunde)" + elapsedTime);
        }

        public static void Menu()
        {
            Console.WriteLine("1.Insert into BulksPassword just password(withoud MD5,SHA1,SHA2");
            Console.WriteLine("2.Delete all rows from BulksPassword");
            Console.WriteLine("3.Bulk insert password to BulksPasswords");
            Console.WriteLine("4.Find a password from Hash");
            int input = Convert.ToInt32(Console.ReadLine());
            if (input == 1)
                insertJustPassword();
            else
                if (input == 2)
                deleteRecordsFromBulksPassword();
            else
                if (input == 3)
                BulkInsertIntoBulksPassword();
            else
                if (input == 4)
                Menu4();
        }

        public static void Menu4()
        {
            Console.WriteLine("1.MD5");
            Console.WriteLine("2.SHA1");
            Console.WriteLine("3.SHA2");
            int input = Convert.ToInt32(Console.ReadLine());
            if (input == 1)
                Search("MD5", "F95711F55E3EA3DE083DE071C8D4841E");
            else
                if (input == 2)
                Search("SHA1", "E4E833B67EE1E4251F9145DE017A4D43175E4BB5");
            else
                Search("SHA2", "6970fe50725f7e012c665f33e12a421b6434e9214f045c455ff87225a891d4ea");


        }

        static void Main(string[] args)
        {
             Menu();
            Console.ReadLine();
          
        }
    }
    }

