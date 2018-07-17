using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.EventArguments
{
    public delegate void OnlineUserEventHandler(object sender, OnlineUserEventArgs e);

    public class OnlineUserEventArgs : EventArgs
    {
        public string User;
        public string Action;

        public OnlineUserEventArgs(string action, string user)
        {
            this.User = user;
            this.Action = action;
        }
    }
}
