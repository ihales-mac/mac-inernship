using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils.EventArguments
{
        public delegate void ClientListRequestedEventHandler(object sender, EventArguments.ClientListRequestedEventArgs e);
        public class ClientListRequestedEventArgs : EventArgs
        {
            public ClientListRequestedEventArgs()
            {
            }
        }
 
}
