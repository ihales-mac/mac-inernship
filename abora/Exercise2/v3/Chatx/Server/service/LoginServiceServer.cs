using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class LoginServiceServer
    {
        private List<Tuple<string, string>> Auth = new List<Tuple<string, string>>();

        public LoginServiceServer()
        {
            Auth.Add(Tuple.Create("andrei", "1"));
            Auth.Add(Tuple.Create("goe", "2"));
            Auth.Add(Tuple.Create("mara", "3"));
        }
        public bool CheckLogin(string username, string password)
        {
            foreach(Tuple<string,string> t in Auth)
            {
                if(t.Item1.Equals(username) && t.Item2.Equals(password))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
