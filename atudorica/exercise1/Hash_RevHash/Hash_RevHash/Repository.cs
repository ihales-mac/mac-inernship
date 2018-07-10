using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hash_RevHash
{
    class Repository
    {
        private exercise2Entities1 db = new exercise2Entities1();
        public void SingleLine(string password)
        {
            Hash h = new Hash();
            h.Password = password;
            h.Md5Hash = HashMethods.GetMd5Hash(password);
            h.Sha1Hash = HashMethods.GetSHA1Hash(password);
            h.Sha2Hash=HashMethods.GetSHA2Hash(password);
            db.Hashes.Add(h);
            db.SaveChanges(); 
        }
    }
}
