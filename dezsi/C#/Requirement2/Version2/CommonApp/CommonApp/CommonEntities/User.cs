using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp.Model
{

    public class User
    {
        public User(string Password, string Username)
        {
            this.Password = Password;
            this.Username = Username;
        }

        public string Password { get; set; }
        public string Username { get; set; }
    }




}
