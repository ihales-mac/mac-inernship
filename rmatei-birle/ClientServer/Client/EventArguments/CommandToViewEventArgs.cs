using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.EventArguments
{
    public delegate void CommandToViewEventHandler(object sender, CommandToViewEventArgs e);

    public class CommandToViewEventArgs : EventArgs
    {
    }
}
