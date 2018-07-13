using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public class Users
    {


        private static Dictionary<string, string> _UsersDict = new Dictionary<string, string>{
            { "user", "1a1dc91c907325c69271ddf0c944bc72" },
            {"mary", "1a1dc91c907325c69271ddf0c944bc72" },
            {"johnny","1a1dc91c907325c69271ddf0c944bc72" }
        };


        public static Dictionary<string, string> UsersDict
        {
            get { return _UsersDict; }

        }

    }
}
