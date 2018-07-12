using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace Client.EventArguments
{
    public delegate void IncomingMessageEventHandler(object sender, IncomingMessageEventArgs e);

    public class IncomingMessageEventArgs : EventArgs
    {
        public string Message { get; }

        public IncomingMessageEventArgs(string message)
        {
            this.Message = message;
        }

    }
}
