using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using EntityFramework.BulkInsert.Extensions;



namespace HashingEntity
{
    public class Insertions
    {

      
        public Insertions(int passlen = 5)
        {

            arrangements = Generate.GenerateArrangements(passlen);
        }
        public enum Loading
        {
            OneInsertEach,
            InsertSome,
            AddRange,
            MultipleRows,
            BulkInsert
        };
        List<string> arrangements;

        public void InsertSingleRowMultipleTimes(int n)
        {

            HashingEntitiesConn context = new HashingEntitiesConn();
            context.Database.Log += Console.WriteLine;
            int i = 0;
            try
            {

                // context.Configuration.AutoDetectChangesEnabled = false;
               
                foreach (string password in arrangements)
                {
                    Console.WriteLine(password);
                    if (i == n)
                        break;
                    string md5hash = HashesClass.computeHash(password, "MD5");
                    string sha1hash = HashesClass.computeHash(password, "SHA1");
                    string sha2hash = HashesClass.computeHash(password, "SHA256");

                    var rainbow = new Rainbow()
                    {
                        password = password,
                        md5hash = md5hash,
                        sha1hash = sha1hash,
                        sha2hash = sha2hash

                    };
                    context.Rainbows.Add(rainbow);
                    //context.SaveChanges();
                    i++;
                }

                context.SaveChanges();
         
            }
            finally
            {
                //    context.Configuration.AutoDetectChangesEnabled = true;
            }

        }

        //inserts all
        public void OneInsertEach()
        {
            HashingEntitiesConn context = new HashingEntitiesConn();
            context.Database.Log += Console.WriteLine;
            try
            {

                context.Configuration.AutoDetectChangesEnabled = false;

                foreach (string password in arrangements)
                {
                    Console.WriteLine(password);

                    string md5hash = HashesClass.computeHash(password, "MD5");
                    string sha1hash = HashesClass.computeHash(password, "SHA1");
                    string sha2hash = HashesClass.computeHash(password, "SHA256");

                    var rainbow = new Rainbow()
                    {
                        password = password,
                        md5hash = md5hash,
                        sha1hash = sha1hash,
                        sha2hash = sha2hash

                    };
                    context.Rainbows.Add(rainbow);
                    //context.SaveChanges();

                }

               context.SaveChanges();
            }
            finally
            {
                context.Configuration.AutoDetectChangesEnabled = true;
            }


        }

        public void AddRange()
        {


            HashingEntitiesConn context = null;
            try
            {
                context = new HashingEntitiesConn();
                context.Database.Log += Console.WriteLine;
                context.Configuration.AutoDetectChangesEnabled = false;
                List<Rainbow> rainbows = new List<Rainbow>();

                foreach (string password in arrangements)
                {
                    string md5hash = HashesClass.computeHash(password, "MD5");
                    string sha1hash = HashesClass.computeHash(password, "SHA1");
                    string sha2hash = HashesClass.computeHash(password, "SHA256");

                    rainbows.Add(new Rainbow()
                    {
                        password = password,
                        md5hash = md5hash,
                        sha1hash = sha1hash,
                        sha2hash = sha2hash

                    });
                    

                }

                Console.WriteLine("finished loading");

                context.Rainbows.AddRange(rainbows);
                context.SaveChanges();
            }
            finally
            {
                if (context != null)
                    context.Dispose();
            }
        }

        public void BulkInsert() {
            /*
            HashingEntitiesConn context = new HashingEntitiesConn();
            IList<Rainbow> rainbows = new List<Rainbow>();
            foreach (string password in arrangements)
            {
                string md5hash = HashesClass.computeHash(password, "MD5");
                string sha1hash = HashesClass.computeHash(password, "SHA1");
                string sha2hash = HashesClass.computeHash(password, "SHA256");

                rainbows.Add(new Rainbow());
            }
        context.BulkInsert<Rainbow>( rainbows, batchSize: 200);
            */
            

        }

        public void LoadIntoDatabase(Loading type)
        {
            if (type == Loading.OneInsertEach)
            {
                OneInsertEach();
            }

            else if (type == Loading.AddRange)
            {
                AddRange();
            }
            else if (type == Loading.BulkInsert)
            {
                BulkInsert();
            }
            else if (type == Loading.InsertSome)
            {
                InsertSingleRowMultipleTimes(200);
            }

        }


    }
}
