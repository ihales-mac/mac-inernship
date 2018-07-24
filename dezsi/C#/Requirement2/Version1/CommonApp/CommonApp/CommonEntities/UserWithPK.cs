using CommonApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp.Model
{
    class UserWithPK : User
    {

        public RSAParameters Key;
        public UserWithPK(string Password, string Username, RSAParameters param) : base(Password, Username)
        {
            Key = param;
        }
    }
}
