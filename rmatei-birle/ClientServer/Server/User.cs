using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class User
    {
        private readonly string Username;
        private readonly string Password;
        public string IC { get; set; }

        public User(string un, string pw)
        {
            this.Username = un;
            this.Password = pw;
        }

        public bool Equals(string username, string password)
        {
            if (username == this.Username && password == this.Password)
            {
                return true;
            }
            return false;
        }

        public string GetUsername()
        {
            return this.Username;
        }

    }
}
