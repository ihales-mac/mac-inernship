using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public class Users
    {

        private static Dictionary<string, Nullable<RSAParameters>> _userKeys = new Dictionary<string, Nullable<RSAParameters>> {
            { "user",null },
            {"mary", null},
            {"johnny",null}

        };
        private static Dictionary<string, string> _UsersDict = new Dictionary<string, string>{
            { "user", "1a1dc91c907325c69271ddf0c944bc72" },
            {"mary", "1a1dc91c907325c69271ddf0c944bc72" },
            {"johnny","1a1dc91c907325c69271ddf0c944bc72" }
        };

        public static Dictionary<string, Nullable<RSAParameters>> GetUserKeys()
        {
          return _userKeys;
            

        }
        public static void  SetUserKey(string user, Nullable<RSAParameters> param)
        {
            if (_userKeys.ContainsKey(user))
                _userKeys[user] = param;

        }

        public static Dictionary<string, string> UsersDict
        {
            get { return _UsersDict; }

        }

    }
}
