using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils.EventArguments
{
    public delegate void ClientDisconnectEventHandler(object sender, ClientDisconnectEventArgs e);
    public class ClientDisconnectEventArgs : EventArgs
    {
        public ClientDisconnectEventArgs()
        {
        }
    }
}
