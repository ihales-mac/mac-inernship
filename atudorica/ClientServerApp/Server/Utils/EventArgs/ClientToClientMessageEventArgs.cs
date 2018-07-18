using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils.EventArguments
{
    public delegate void ClientToClientMessageEventHandler(object sender, EventArguments.ClientToClientMessageEventArgs e);
    public class ClientToClientMessageEventArgs : EventArgs
    {
        public string sender { get; set; }
        public string receiver { get; set; }
        public string message { get; set; }
        public ClientToClientMessageEventArgs(string sender,string receiver,string message)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.message = message;
        }
    }
}
