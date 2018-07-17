using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.EventArguments
{
    public delegate void CommandRemoveToViewEventHandler(object sender, CommandRemoveToViewEventArgs e);

    public class CommandRemoveToViewEventArgs : EventArgs
    {
        public string User;

        public CommandRemoveToViewEventArgs(string user)
        {
            this.User = user;
        }
    }
}
