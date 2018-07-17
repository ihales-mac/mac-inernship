using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.EventArguments
{
    public delegate void LoginEventHandler(object sender, LoginEventArgs e);

    public class LoginEventArgs
    {
        public string Response;

        public LoginEventArgs(string resp)
        {
            this.Response = resp;
        }
    }
}
