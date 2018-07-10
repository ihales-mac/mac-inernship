using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowEF
{
    class Repository
    {
        public Repository()
        {
            List<RainbowItem> rainbowItems = new List<RainbowItem>();
            List<string> permutations = Permutation.getPermutations(2);
            foreach (string p in permutations)
            {
                rainbowItems.Add(
                    new RainbowItem()
                    {
                        Password = p,
                        Md5Hash = Hashing.EncodeMD5(p),
                        Sha1Hash = Hashing.EncodeSHA1(p),
                        Sha256Hash = Hashing.EncodeSHA256(p)
                    }
                    );

            }

            //v1
            /*
             * For each item it uses an insert
             * Add the items in on implicit transaction
             */

            var watch = System.Diagnostics.Stopwatch.StartNew();
            using (RainbowContext ctx = new RainbowContext())
            {
                //ctx.Database.Log = Console.Write;
                ctx.RainbowItems.AddRange(rainbowItems);
                ctx.SaveChanges();
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Populate table in {0} ms", elapsedMs);


            //v2
            /*
             * For each item it uses an insert
             * Add the items in on explicit transaction
             */
            /*
           var watch = System.Diagnostics.Stopwatch.StartNew();
           using (RainbowContext ctx = new RainbowContext())
           {
               //ctx.Database.Log = Console.Write;
               using (var dbContextTransaction = ctx.Database.BeginTransaction())
               {
                   try
                   {

                       ctx.RainbowItems.AddRange(rainbowItems);
                       ctx.SaveChanges();
                       dbContextTransaction.Commit();
                   }catch(Exception e)
                   {
                       dbContextTransaction.Rollback();
                   }

               }

           }
           watch.Stop();
           var elapsedMs = watch.ElapsedMilliseconds;
           Console.WriteLine("Populate table in {0} ms", elapsedMs);
           */
        }


        public void Add(string username, string password, string hashAlgorithm)
        {
            string hashPassword = null;
            if (hashAlgorithm.ToUpper().Equals("MD5"))
            {
                hashPassword = Hashing.EncodeMD5(password);
            }else if (hashAlgorithm.ToUpper().Equals("SHA1"))
            {
                hashPassword = Hashing.EncodeSHA1(password);
            }else if (hashAlgorithm.ToUpper().Equals("SHA256"))
            {
                hashPassword = Hashing.EncodeSHA256(password);
            }

            using (RainbowContext ctx = new RainbowContext())
            {
                User user = new User()
                {
                    Username = username,
                    HashedPassword = hashPassword

                };
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
        }

        public string GetPassword(string username)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            string rawPassword = null;
            using (RainbowContext ctx = new RainbowContext())
            {
                string hashedPassword = ctx.Users.Where(u => u.Username.Equals(username)).FirstOrDefault().HashedPassword;
                rawPassword = ctx.RainbowItems
                    .Where(item => item.Md5Hash.Equals(hashedPassword) || item.Sha1Hash.Equals(hashedPassword) || item.Sha256Hash.Equals(hashedPassword))
                    .FirstOrDefault().Password;
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Find password in {0} ms", elapsedMs);
            return rawPassword;
        }
    }
}
