using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.EventArguments
{
    public delegate void CommandAddToViewEventHandler(object sender, CommandAddToViewEventArgs e);

    public class CommandAddToViewEventArgs : EventArgs
    {
        public string User;

        public CommandAddToViewEventArgs(string user)
        {
            this.User = user;
        }
    }
}
