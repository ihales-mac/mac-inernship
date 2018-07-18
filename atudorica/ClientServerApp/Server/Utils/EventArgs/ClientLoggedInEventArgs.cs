using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils.EventArguments
{
    public delegate void ClientLoggedInEventHandler(object sender, ClientLoggedInEventArgs e);
    public class ClientLoggedInEventArgs : EventArgs
    {
        public string Username;
        public ClientLoggedInEventArgs(string username)
        {
            this.Username = username;
        }
    }
}
