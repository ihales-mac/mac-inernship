using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerUtils
    {
        private readonly List<Tuple<string, string>> _accounts;
        public ServerUtils()
        {
            _accounts = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("andrei", "andrei"),
                new Tuple<string, string>("admin", "admin"),
                new Tuple<string, string>("student", "student"),
                new Tuple<string, string>("matei", "matei")
            };
        }
        public bool CheckCredentials(string username,string password)
        {
            for (int j = 0; j < _accounts.Count; j++)
                if (_accounts[j].Item1 == username && _accounts[j].Item2 == password)
                    return true;
            return false;
        }
    }
}
