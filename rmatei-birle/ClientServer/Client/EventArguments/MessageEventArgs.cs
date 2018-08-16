using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.EventArguments
{
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    public class MessageEventArgs : EventArgs
    {
        public Tuple<string, string, string> Message;

        public MessageEventArgs(Tuple<string, string, string> msg)
        {
            this.Message = msg;
        }
    }
}
