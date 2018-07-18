using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    public class Users
    {

        public Users()
        {
            _UsersDict.Add("user", "1a1dc91c907325c69271ddf0c944bc72");
            _UsersDict.Add("uer2", "1a1dc91c907325c69271ddf0c944bc72");

        }
        private Dictionary<string, string> _UsersDict = new Dictionary<string, string>();


        public Dictionary<string, string> UsersDict
        {
            get { return _UsersDict; }

        }

    }
}
