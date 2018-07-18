using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utils.EventArguments
{
        public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public class MessageReceivedEventArgs : EventArgs
        {
            public Message m { get; set; }

            public MessageReceivedEventArgs(Message m)
            {
                this.m = m;
            }
        }
}
