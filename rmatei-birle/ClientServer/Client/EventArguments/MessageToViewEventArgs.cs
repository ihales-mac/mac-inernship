using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.EventArguments
{
    public delegate void MessageToViewEventHandler(object sender, MessageToViewEventArgs e);

    public class MessageToViewEventArgs : EventArgs
    {
        public string Username;

        public MessageToViewEventArgs(string username)
        {
            this.Username = username;
        }
    }
}
